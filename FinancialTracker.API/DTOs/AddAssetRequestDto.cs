using FinancialTracker.API.Entities;

namespace FinancialTracker.API.DTOs;

public class AddAssetRequestDto
{
    public string Symbol { get; set; } = string.Empty;
    public AssetType AssetType { get; set; } = AssetType.Stock;
    public decimal Quantity { get; set; }
    public decimal? TargetPrice { get; set; }
    public decimal? AverageCost { get; set; }
}
