using FinancialTracker.MVC.Models;

namespace FinancialTracker.MVC.Services;

public class ApiClientService : IApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClientService> _logger;

    public ApiClientService(HttpClient httpClient, ILogger<ApiClientService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> AddAssetAsync(AddAssetFormModel formModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("assets", formModel, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned {StatusCode} while adding asset.", response.StatusCode);
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding asset to API.");
            return false;
        }
    }

    public async Task<bool> DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"assets/{id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned {StatusCode} while deleting asset {Id}", response.StatusCode, id);
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting asset {Id}", id);
            return false;
        }
    }

    public async Task<DashboardPageViewModel> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {

        var viewModel = new DashboardPageViewModel();
        try
        {
            var response = await _httpClient.GetAsync("assets/dashboard", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned unsuccessful status code: {StatusCode}", response.StatusCode);
                return viewModel;
            }

            var data = await response.Content.ReadFromJsonAsync<List<DashboardViewModel>>(cancellationToken: cancellationToken);
            if (data == null)
            {
                return viewModel;
            }
            viewModel.Assets = data;
            viewModel.TotalValue = data.Sum(a => a.TotalValue);
            viewModel.NearTargetCount = data.Count(a => a.IsNearTarget);

            foreach (var asset in data)
            {
                if (asset.ProfitLoss.HasValue)
                {
                    viewModel.TotalProfit += asset.ProfitLoss.Value;
                }
            }

            viewModel.AssetTypes = new List<AssetTypeOption>
            {
                    new AssetTypeOption 
                    { 
                        Value = 10, 
                        Name = "Stock" 
                    },
                    new AssetTypeOption 
                    { 
                        Value = 20, 
                        Name = "Forex"
                    },
                    new AssetTypeOption 
                    { 
                        Value = 30,
                        Name = "Crypto" 
                    }
            };

            return viewModel;


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "A critical error occurred while communicating with the API.");
            return viewModel;
        }
    }

    public async Task<bool> SyncPricesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync("assets/refresh", null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned {StatusCode} while syncing prices.", response.StatusCode);
                return false;
            }
            return true;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while syncing prices.");
            return false;
        }
    }
}
