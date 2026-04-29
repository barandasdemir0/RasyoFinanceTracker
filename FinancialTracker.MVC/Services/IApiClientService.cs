using FinancialTracker.MVC.Models;

namespace FinancialTracker.MVC.Services;

public interface IApiClientService
{
    Task<List<DashboardViewModel>> GetDashboardDataAsync(CancellationToken cancellationToken = default);
}
