using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.API.Entities;

public class TrackedAsset
{
    public TrackedAsset()
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTime.UtcNow;
        PriceSnapshots = new List<PriceSnapshot>();
    }
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public AssetType AssetType { get; set; } = AssetType.Stock;
    public decimal Quantity { get; set; }
    public decimal? AverageCost { get; set; }
    public decimal? TargetPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    [NotMapped]
    public bool IsWatchlistOnly  => Quantity == 0;
    public ICollection<PriceSnapshot> PriceSnapshots { get; set; }
}
