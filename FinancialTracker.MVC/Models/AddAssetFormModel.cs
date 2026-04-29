namespace FinancialTracker.MVC.Models;

public class AddAssetFormModel
{
    public string Symbol { get; set; } = string.Empty;
    public int AssetType { get; set; }
    public decimal Quantity { get; set; }
    public decimal? AverageCost { get; set; }
    public decimal? TargetPrice { get; set; }
}
