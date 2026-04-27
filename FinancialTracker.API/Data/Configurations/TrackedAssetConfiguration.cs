using FinancialTracker.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialTracker.API.Data.Configurations;

public class TrackedAssetConfiguration : IEntityTypeConfiguration<TrackedAsset>
{
    public void Configure(EntityTypeBuilder<TrackedAsset> builder)
    {
        //primary key ayarı yaptım
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Symbol).IsUnique();

        builder.Property(t => t.Symbol)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.AssetType)
            .HasConversion<string>()
            .HasMaxLength(20);


        builder.Property(t => t.Quantity).HasPrecision(18, 4);
        builder.Property(t => t.AverageCost).HasPrecision(18, 4);
        builder.Property(t => t.TargetPrice).HasPrecision(18, 4);

    }
}
