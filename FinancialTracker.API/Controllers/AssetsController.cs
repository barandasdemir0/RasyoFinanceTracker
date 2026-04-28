using FinancialTracker.API.DTOs;
using FinancialTracker.API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    private readonly IValidator<AddAssetRequestDto> _validator;

    public AssetsController(IPortfolioService portfolioService, IValidator<AddAssetRequestDto> validator)
    {
        _portfolioService = portfolioService;
        _validator = validator;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var dashboardData = await _portfolioService.GetDashboardAsync(cancellationToken);
        return Ok(dashboardData);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsset([FromBody] AddAssetRequestDto addAssetRequestDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(addAssetRequestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _portfolioService.AddAssetAsync(addAssetRequestDto, cancellationToken);
        return StatusCode(201);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshPrices(CancellationToken cancellationToken)
    {
        await _portfolioService.RefreshPricesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("id:guid")]
    public async Task<IActionResult> DeleteAsset(Guid id,CancellationToken cancellationToken)
    {
        await _portfolioService.DeleteAssetAsync(id, cancellationToken);

        return NoContent();
    }


}
