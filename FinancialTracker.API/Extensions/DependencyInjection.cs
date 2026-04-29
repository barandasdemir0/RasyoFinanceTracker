using FinancialTracker.API.Data;
using FinancialTracker.API.Repositories;
using FinancialTracker.API.Services;
using FinancialTracker.API.Services.External;
using FinancialTracker.API.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinancialTracker.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ITrackedAssetRepository, TrackedAssetRepository>();

        services.Configure<FinnhubSettings>(configuration.GetSection("FinnhubSettings"));

        services.AddHttpClient<IFinancialDataProvider, FinnhubDataProvider>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<FinnhubSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);

        });

        services.AddValidatorsFromAssemblyContaining<AddAssetRequestDtoValidator>();
        services.AddScoped<IPortfolioService, PortfolioService>();

        services.AddSignalR();
        services.AddHostedService<PriceRefreshBackgroundService>();


        var origin = configuration["MvcSettings:BaseUrl"];

        if (string.IsNullOrEmpty(origin))
        {
            throw new Exception("MvcSettings:BaseUrl is missing!");
        }

        services.AddCors(options =>
        {
            options.AddPolicy("AllowMvc", policy =>
            {
                policy.WithOrigins(origin).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
        });



        return services;

    }
}
