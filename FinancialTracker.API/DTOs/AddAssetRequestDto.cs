namespace FinancialTracker.API.DTOs;

public class AddAssetRequestDto
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? TargetPrice { get; set; }
}
