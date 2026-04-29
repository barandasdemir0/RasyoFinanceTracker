using FinancialTracker.MVC.Models;
using FinancialTracker.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IApiClientService _apiClientService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IApiClientService apiClientService, ILogger<HomeController> logger)
    {
        _apiClientService = apiClientService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dashboardData = await _apiClientService.GetDashboardDataAsync(cancellationToken);
        return View(dashboardData);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAssetFormModel formModel,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var dashboardData = await _apiClientService.GetDashboardDataAsync(cancellationToken);
            return View(nameof(Index),dashboardData);
        }

        var success = await _apiClientService.AddAssetAsync(formModel, cancellationToken);
        if (!success)
        {
            _logger.LogWarning("Failed to add asset {Symbol}", formModel.Symbol);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation)
    {
        var success = await _apiClientService.DeleteAssetAsync(id, cancellation);
        if (!success)
        {
            _logger.LogWarning("Failed to delete asset {Id}", id);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var success = await _apiClientService.SyncPricesAsync(cancellationToken);
        if (!success)
        {
            _logger.LogWarning("Failed to sync prices.");
        }
        return RedirectToAction(nameof(Index));
    }

   
}
