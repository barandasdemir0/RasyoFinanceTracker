using FinancialTracker.API.Entities;
using FinancialTracker.API.Repositories.Models;

namespace FinancialTracker.API.Repositories;

public interface ITrackedAssetRepository
{
    // Using IReadOnlyList to prevent accidental additions/modifications from the service layer
    Task<IReadOnlyList<TrackedAsset>> GetAllAsync(CancellationToken cancellationToken = default);

    // Optimized for dashboard: fetches all assets but only maps their latest price
    Task<IReadOnlyList<AssetWithLatestPriceResult>> GetAllWithLatestPriceAsync(CancellationToken cancellationToken);

    Task<TrackedAsset?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);

    // Pulls the asset with its full price history 
    Task<TrackedAsset?> GetByIdWithSnapshotAsync(Guid id, CancellationToken cancellationToken = default);

  
    Task AddAsync(TrackedAsset trackedAsset, CancellationToken cancellationToken = default);
    void Update(TrackedAsset trackedAsset);
    void Delete(TrackedAsset trackedAsset);

    // Appends a new price point history without modifying the parent asset
    Task AddSnapshotAsync(PriceSnapshot snapshot, CancellationToken cancellationToken = default);


    Task SaveChangesAsync(CancellationToken cancellationToken = default);

}
