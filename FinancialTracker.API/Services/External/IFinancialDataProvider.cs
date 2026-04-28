namespace FinancialTracker.API.Services.External;


// DESIGN PATTERN: Strategy Pattern
// WHY: We used the Strategy Pattern to abstract the external financial API logic. 
// If we decide to switch from Finnhub to AlphaVantage in the future, we only need to write a new class 
// implementing this interface, without touching the PortfolioService.
public interface IFinancialDataProvider
{
    //It takes the stock symbol (e.g., AAPL) and returns the current price in decimal format.
    Task<decimal> GetCurrentPriceAsync(string symbol, CancellationToken cancellationToken=default);
}
