namespace FinancialTracker.MVC.Models;

public class DashboardPageViewModel
{
    public List<DashboardViewModel> Assets { get; set; } = new();
    public List<AssetTypeOption> AssetTypes { get; set; } = new();
    public decimal TotalValue { get; set; }
    public decimal TotalProfit { get; set; }
    public int NearTargetCount { get; set; }
}
