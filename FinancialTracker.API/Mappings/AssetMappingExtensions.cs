using FinancialTracker.API.DTOs;
using FinancialTracker.API.Entities;

namespace FinancialTracker.API.Mappings;

public static class AssetMappingExtensions
{
    private const decimal TargetThreshold = 0.95m;

    public static DashboardAssetResponseDto ToDashboardDto(this TrackedAsset trackedAsset, PriceSnapshot? latestPrice)
    {
        decimal currentPrice;
        if (latestPrice != null && latestPrice?.Price != null)
        {
            currentPrice = latestPrice.Price;
        }
        else
        {
            currentPrice = 0m;
        }

        bool isNearTarget = false;
        if (trackedAsset.TargetPrice.HasValue && currentPrice >= (trackedAsset.TargetPrice.Value * TargetThreshold))
        {
            isNearTarget = true;
        }

        decimal? profitOrLoss = null;
        if (trackedAsset.AverageCost.HasValue && trackedAsset.Quantity > 0)
        {
            profitOrLoss = (currentPrice - trackedAsset.AverageCost.Value) * trackedAsset.Quantity;
        }

        return new DashboardAssetResponseDto
        {
            Id = trackedAsset.Id,
            Symbol = trackedAsset.Symbol,
            AssetType = trackedAsset.AssetType.ToString(),
            Quantity = trackedAsset.Quantity,
            AverageCost = trackedAsset.AverageCost,
            CurrentPrice = currentPrice,
            TargetPrice = trackedAsset.TargetPrice,
            IsWatchlistOnly = trackedAsset.Quantity == 0,
            IsNearTarget = isNearTarget,
            TotalValue = trackedAsset.Quantity * currentPrice,
            ProfitLoss = profitOrLoss,
        };


    }

    public static TrackedAsset ToEntity(this AddAssetRequestDto requestDto, string normalizedSymbol)
    {
        var newAsset = new TrackedAsset();

        newAsset.Symbol = normalizedSymbol;
        newAsset.AssetType = requestDto.AssetType;
        newAsset.Quantity = requestDto.Quantity;
        newAsset.TargetPrice = requestDto.TargetPrice;
        if (requestDto.Quantity > 0)
        {
            newAsset.AverageCost = requestDto.AverageCost;
        }
        else
        {
            newAsset.AverageCost = null;
        }
        return newAsset;






    }




}
