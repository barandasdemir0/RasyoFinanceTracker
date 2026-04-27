using FinancialTracker.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialTracker.API.Data.Configurations;

public class PriceSnapshotConfiguration : IEntityTypeConfiguration<PriceSnapshot>
{
    public void Configure(EntityTypeBuilder<PriceSnapshot> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Price).HasPrecision(18, 4);
        builder.Property(p => p.DailyHigh).HasPrecision(18, 4);
        builder.Property(p => p.DailyLow).HasPrecision(18, 4);

        //bire çok ilişki sağladık hisse silinirse fiyat geçmisinide silmesi adına
        builder.HasOne(p => p.TrackedAsset)
            .WithMany(t => t.PriceSnapshots)
            .HasForeignKey(p => p.TrackedAssetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
