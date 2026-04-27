using FinancialTracker.API.Entities;

namespace FinancialTracker.API.Repositories.Models;

public record AssetWithLatestPriceResult(
    TrackedAsset Asset,
    PriceSnapshot? LatestPrice
    );
