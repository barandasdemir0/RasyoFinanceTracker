using FinancialTracker.API.Data;
using FinancialTracker.API.Entities;
using FinancialTracker.API.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.API.Repositories;


// DESIGN PATTERN: Repository Pattern
// WHY: Used to decouple the data access logic (Entity Framework Core) from the business layer. 
// It ensures the Services remain ignorant of the database implementation and makes unit testing easier.

public class TrackedAssetRepository : ITrackedAssetRepository
{
    private readonly AppDbContext _appDbContext;

    public TrackedAssetRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(TrackedAsset trackedAsset, CancellationToken cancellationToken = default)
    {
        await _appDbContext.TrackedAssets.AddAsync(trackedAsset, cancellationToken);
    }

    public async Task AddSnapshotAsync(PriceSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        await _appDbContext.PriceSnapshots.AddAsync(snapshot, cancellationToken);
    }

    public void Delete(TrackedAsset trackedAsset)
    {
        _appDbContext.TrackedAssets.Remove(trackedAsset);
    }

    public async Task<IReadOnlyList<TrackedAsset>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // AsNoTracking used for read-only performance
        return await _appDbContext.TrackedAssets.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AssetWithLatestPriceResult>> GetAllWithLatestPriceAsync(CancellationToken cancellationToken)
    {

        // Using projection (.Select) to avoid pulling thousands of historical prices into memory
        return await _appDbContext.TrackedAssets
            .AsNoTracking()
            .Select(asset => new AssetWithLatestPriceResult(
            asset,
            asset.PriceSnapshots.OrderByDescending(p => p.FetchedAt).FirstOrDefault())).ToListAsync(cancellationToken);
    }

    public async Task<TrackedAsset?> GetByIdWithSnapshotAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TrackedAssets.Include(t => t.PriceSnapshots).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TrackedAsset?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TrackedAssets.FirstOrDefaultAsync(t => t.Symbol == symbol, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Update(TrackedAsset trackedAsset)
    {
        _appDbContext.TrackedAssets.Update(trackedAsset);
    }
}
