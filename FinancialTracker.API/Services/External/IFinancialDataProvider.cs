namespace FinancialTracker.API.Services.External;


// DESIGN PATTERN: Strategy Pattern
// WHY: Defines a common contract for external financial data providers. 
// It allows us to easily swap out Finnhub for AlphaVantage or any other API in the future without modifying our business logic.
public interface IFinancialDataProvider
{
    //It takes the stock symbol (e.g., AAPL) and returns the current price in decimal format.
    Task<decimal> GetCurrentPriceAsync(string symbol, CancellationToken cancellationToken=default);
}
