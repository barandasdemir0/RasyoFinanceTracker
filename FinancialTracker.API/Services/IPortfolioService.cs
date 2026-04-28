using FinancialTracker.API.DTOs;

namespace FinancialTracker.API.Services;

public interface IPortfolioService
{
    Task AddAssetAsync(AddAssetRequestDto requestDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<DashboardAssetResponseDto>> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task RefreshPricesAsync(CancellationToken cancellationToken = default);
    Task DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default);
}
