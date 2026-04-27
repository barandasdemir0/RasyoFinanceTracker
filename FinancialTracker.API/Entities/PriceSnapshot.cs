using Microsoft.EntityFrameworkCore;


namespace FinancialTracker.API.Entities;

// Composite index to speed up history queries and avoid full table scans

[Index(nameof(TrackedAssetId), nameof(FetchedAt))]
public class PriceSnapshot
{
    
    public PriceSnapshot()
    {
        Id = Guid.CreateVersion7();
       
    }
    public Guid Id { get; set; }

    // Foreign Key mapping
    public Guid TrackedAssetId { get; set; }
    public TrackedAsset? TrackedAsset { get; set; }

    public decimal Price { get; set; }
    public decimal? DailyHigh { get; set; }
    public decimal? DailyLow { get; set; }

    // Storing as UTC to avoid timezone headaches later
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

}
