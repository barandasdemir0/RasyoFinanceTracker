using FinancialTracker.MVC.Models;

namespace FinancialTracker.MVC.Services;

public interface IApiClientService
{
    Task<DashboardPageViewModel> GetDashboardDataAsync(CancellationToken cancellationToken = default);
    Task<bool> AddAssetAsync(AddAssetFormModel formModel, CancellationToken cancellationToken = default);
    Task<bool> DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SyncPricesAsync(CancellationToken cancellationToken = default);
}
