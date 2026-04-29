using FinancialTracker.API.Entities;
using FinancialTracker.API.Mappings;

namespace FinancialTracker.Tests;

public class AssetMappingExtensionsTests
{
    [Fact]
    public void ToDashboardDto_ShouldCalculateProfitLoss_Correctly()
    {
        // Arrange (Hazırlık)
        var asset = new TrackedAsset
        {
            Symbol = "AAPL",
            Quantity = 10,
            AverageCost = 150m, // 150$'dan 10 tane aldık = 1500$ maliyet
            TargetPrice = 200m
        };
        var snapshot = new PriceSnapshot
        {
            Price = 180m // Fiyat 180$'a çıktı. Beklenen Kâr: (180 - 150) * 10 = 300$
        };

        var dto = asset.ToDashboardDto(snapshot);

        Assert.Equal(300m, dto.ProfitLoss);
        Assert.Equal(1800m, dto.TotalValue);
        Assert.False(dto.IsNearTarget);

    }
}
