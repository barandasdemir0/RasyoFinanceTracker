using FinancialTracker.API.Extensions;
using FinancialTracker.API.Hubs;
using FinancialTracker.API.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowMvc");

app.UseAuthorization();



app.MapControllers();
app.MapHub<DashboardHub>("/hubs/dashboard");



app.Run();
