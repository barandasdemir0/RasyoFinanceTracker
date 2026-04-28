using FinancialTracker.API.DTOs;
using FinancialTracker.API.Entities;
using FinancialTracker.API.Exceptions;
using FinancialTracker.API.Mappings;
using FinancialTracker.API.Repositories;
using FinancialTracker.API.Services.External;

namespace FinancialTracker.API.Services;

public class PortfolioService : IPortfolioService
{
    private readonly ITrackedAssetRepository _repository;
    private readonly IFinancialDataProvider _financialDataProvider;
    private readonly ILogger<PortfolioService> _logger;

    public PortfolioService(ITrackedAssetRepository repository, IFinancialDataProvider financialDataProvider, ILogger<PortfolioService> logger)
    {
        _repository = repository;
        _financialDataProvider = financialDataProvider;
        _logger = logger;
    }

    public async Task AddAssetAsync(AddAssetRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var normalizedSymbol = requestDto.Symbol.Trim().ToUpper();

        var currentAsset = await _repository.GetBySymbolAsync(normalizedSymbol, cancellationToken);
        if (currentAsset != null)
        {
            throw new InvalidOperationException($"Asset with symbol {normalizedSymbol} is already being tracked.");
        }

        var currentPrice = await _financialDataProvider.GetCurrentPriceAsync(normalizedSymbol, cancellationToken);

        var newAsset = requestDto.ToEntity(normalizedSymbol);

        await _repository.AddAsync(newAsset, cancellationToken);
        var initialSnapshot = new PriceSnapshot
        {
            TrackedAssetId = newAsset.Id,
            Price = currentPrice
        };

        await _repository.AddSnapshotAsync(initialSnapshot, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully added new asset {Symbol} at price {Price}", normalizedSymbol, currentPrice);

    }

    public async Task DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var asset = await _repository.GetByIdWithSnapshotAsync(id, cancellationToken);
        if (asset is null)
        {
            throw new NotFoundException($"Asset with ID {id} not found.");
        }
        _repository.Delete(asset);

        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted asset {Symbol} (ID: {Id})", asset.Symbol, asset.Id);
    }

    public async Task<IEnumerable<DashboardAssetResponseDto>> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var assetsWithLatestPrice = await _repository.GetAllWithLatestPriceAsync(cancellationToken);
        var dashboardlist = assetsWithLatestPrice.Select(result =>
        {
            if (result.LatestPrice == null)
            {
                _logger.LogWarning("No price data found for {Symbol}. Displaying 0 as fallback.", result.Asset.Symbol);
            }

            return result.Asset.ToDashboardDto(result.LatestPrice);

        });
        return dashboardlist;
    }

    public async Task RefreshPricesAsync(CancellationToken cancellationToken = default)
    {
        var allAssets = await _repository.GetAllAsync(cancellationToken);
        var tasks = allAssets.Select(async asset =>
        {
            try
            {
                var price = await _financialDataProvider.GetCurrentPriceAsync(asset.Symbol, cancellationToken);

                return new PriceSnapshot
                {
                    TrackedAssetId = asset.Id,
                    Price = price
                };
            }
            catch (ExternalApiException ex)
            {
                _logger.LogWarning(ex, "Skipping {Symbol}", asset.Symbol);
                return null;
            }
        });

        var snapshots = await Task.WhenAll(tasks);
        foreach (var snapshot in snapshots.Where(s=>s != null))
        {
            await _repository.AddSnapshotAsync(snapshot!, cancellationToken);
        }
        await _repository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully refreshed prices for {Count} assets.", snapshots.Count(s => s != null));
    }
}
