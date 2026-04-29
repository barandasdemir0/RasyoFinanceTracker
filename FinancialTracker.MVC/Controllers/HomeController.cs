using FinancialTracker.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IApiClientService _apiClientService;

    public HomeController(IApiClientService apiClientService)
    {
        _apiClientService = apiClientService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dashboardData = await _apiClientService.GetDashboardDataAsync(cancellationToken);
        return View(dashboardData);
    }

   
}
