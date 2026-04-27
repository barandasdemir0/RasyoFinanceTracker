using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.API.Entities;
// Represents an asset (Stock, Crypto, etc.) being tracked by the user.
public class TrackedAsset
{
    public TrackedAsset()
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTime.UtcNow; 
        PriceSnapshots = new List<PriceSnapshot>();
        // Init to avoid null refs when creating a new asset
    }
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty; 
    public AssetType AssetType { get; set; } = AssetType.Stock;
    public decimal Quantity { get; set; } 
    public decimal? AverageCost { get; set; } 
    public decimal? TargetPrice { get; set; } 
    public DateTime CreatedAt { get; set; } 

    [NotMapped]  // If quantity is 0, we consider this just a watchlist item
    public bool IsWatchlistOnly  => Quantity == 0; 
    public ICollection<PriceSnapshot> PriceSnapshots { get; set; }

}
