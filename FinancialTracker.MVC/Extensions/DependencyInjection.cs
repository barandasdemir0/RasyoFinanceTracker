using FinancialTracker.MVC.Services;

namespace FinancialTracker.MVC.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMvcService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient<IApiClientService, ApiClientService>(client =>
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("ApiSettings:BaseUrl is not configured in appsettings.json");
            }

            client.BaseAddress = new Uri(baseUrl);

        });

        return services;

    }
}
