using FinancialTracker.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinancialTracker.API.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TrackedAsset> TrackedAssets { get; set; }
    public DbSet<PriceSnapshot> PriceSnapshots { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //configurasyon dosyalarımızı çektik
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
