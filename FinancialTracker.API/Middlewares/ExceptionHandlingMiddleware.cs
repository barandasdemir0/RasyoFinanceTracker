using FinancialTracker.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace FinancialTracker.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context,Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            ExternalApiException => (int)HttpStatusCode.BadGateway,
            _ => (int)HttpStatusCode.InternalServerError

        };

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            // Status Code'a göre nokta atışı detay mesajı veriyoruz:
            Detail = context.Response.StatusCode switch
            {
                400 => "The data you submitted is incomplete or incorrect. Please check the fields.",
                401 => "You must log in to the system to perform this action (Invalid Token).",
                403 => "You are not authorized to perform this action.",
                404 => "The record you are trying to process could not be found in the database.",
                500 => "An unexpected error occurred on the server. Please try again later.",
                _ => "An unknown error occurred during your transaction."
            }
        });

        return context.Response.WriteAsync(result);


    }


}
