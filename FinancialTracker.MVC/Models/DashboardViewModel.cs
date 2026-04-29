namespace FinancialTracker.MVC.Models;

public class DashboardViewModel
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal? AverageCost { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal? TargetPrice { get; set; }
    public bool IsWatchlistOnly { get; set; }
    public bool IsNearTarget { get; set; }
    public decimal TotalValue { get; set; }
    public decimal? ProfitLoss { get; set; }
}
