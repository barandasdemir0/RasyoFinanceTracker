namespace FinancialTracker.API.DTOs;

public record AssetPriceResult(decimal CurrentPrice, decimal? DailyHigh, decimal? DailyLow);
