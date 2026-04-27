using FinancialTracker.API.Data;
using FinancialTracker.API.Repositories;
using Microsoft.EntityFrameworkCore;

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


        return services;

    }
}
