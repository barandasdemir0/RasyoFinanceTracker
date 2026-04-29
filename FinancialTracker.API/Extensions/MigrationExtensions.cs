using FinancialTracker.API.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContect = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		try
		{
			//veritabanı yoksa oluştur
			dbContect.Database.Migrate();
		}
		catch (Exception ex)
		{
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw; // Docker'ın çöküp yeniden başlaması (retry) için hatayı fırlatıyoruz
        }
    }
}
