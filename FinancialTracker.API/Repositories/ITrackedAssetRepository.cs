using FinancialTracker.API.Entities;
using FinancialTracker.API.Repositories.Models;

namespace FinancialTracker.API.Repositories;

public interface ITrackedAssetRepository
{
    //listeleme ve getirme metotları IReadOnlyList kullanarak içine veri eklemesini engelledim
    Task<IReadOnlyList<TrackedAsset>> GetAllAsync(CancellationToken cancellationToken = default);

    //en son fiyatları getiren metot
    Task<IReadOnlyList<AssetWithLatestPriceResult>> GetAllWithLatestPriceAsync(CancellationToken cancellationToken);

    Task<TrackedAsset?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);

    //dashboard için geçmişleriyle birlikte getirme

    Task<TrackedAsset?> GetByIdWithSnapshotAsync(Guid id, CancellationToken cancellationToken = default);

    //ekleme güncelleme
    Task AddAsync(TrackedAsset trackedAsset, CancellationToken cancellationToken = default);
    void Update(TrackedAsset trackedAsset);

    //yeni fiyatı ekleme
    Task AddSnapshotAsync(PriceSnapshot snapshot, CancellationToken cancellationToken = default);

    //kaydetme
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

}
