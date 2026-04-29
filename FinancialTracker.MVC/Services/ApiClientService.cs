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

    public async Task<List<DashboardViewModel>> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("portfolio/dashboard", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned unsuccessful status code: {StatusCode}", response.StatusCode);
                return new List<DashboardViewModel>();
            }

            var data = await response.Content.ReadFromJsonAsync<List<DashboardViewModel>>(cancellationToken: cancellationToken);
            if (data==null)
            {
                return new List<DashboardViewModel>();
            }

            return data;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "A critical error occurred while communicating with the API.");
            return new List<DashboardViewModel>();
        }
    }
}
