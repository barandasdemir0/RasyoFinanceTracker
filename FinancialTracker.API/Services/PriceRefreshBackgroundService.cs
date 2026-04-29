using FinancialTracker.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FinancialTracker.API.Services;

public class PriceRefreshBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<DashboardHub> _hubContext;
    private readonly ILogger<PriceRefreshBackgroundService> _logger;

    public PriceRefreshBackgroundService(IServiceScopeFactory serviceScopeFactory, IHubContext<DashboardHub> hubContext, ILogger<PriceRefreshBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price Refresh Background Service started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var portfolioService = scope.ServiceProvider.GetRequiredService<IPortfolioService>();

                await portfolioService.RefreshPricesAsync(stoppingToken);

                await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate");

                _logger.LogInformation("Prices refreshed. Signal sent to all clients.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in price refresh cycle.");
            }

            await Task.Delay(TimeSpan.FromSeconds(60),stoppingToken);
        }
    }


}
