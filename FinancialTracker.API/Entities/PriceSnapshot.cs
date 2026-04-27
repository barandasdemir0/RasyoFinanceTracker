using Microsoft.EntityFrameworkCore;


namespace FinancialTracker.API.Entities;

[Index(nameof(TrackedAssetId), nameof(FetchedAt))]
public class PriceSnapshot
{
    
    public PriceSnapshot()
    {
        Id = Guid.CreateVersion7();
       
    }
    public Guid Id { get; set; }
    public Guid TrackedAssetId { get; set; }

    public TrackedAsset? TrackedAsset { get; set; }
    public decimal Price { get; set; }
    public decimal? DailyHigh { get; set; }
    public decimal? DailyLow { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

}
