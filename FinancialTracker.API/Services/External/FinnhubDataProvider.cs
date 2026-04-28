using FinancialTracker.API.Exceptions;
using System.Text.Json;

namespace FinancialTracker.API.Services.External;

public class FinnhubDataProvider : IFinancialDataProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FinnhubDataProvider> _logger;

    public FinnhubDataProvider(HttpClient httpClient, ILogger<FinnhubDataProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<decimal> GetCurrentPriceAsync(string symbol, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching current price for {Symbol} from Finnhub...", symbol);
        try
        {
            //If there's a problem with the API request, stop the process immediately.
            var response = await _httpClient.GetAsync($"quote?symbol={symbol}", cancellationToken);
            response.EnsureSuccessStatusCode();

            //Read the incoming data and break it down into parts.
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            using var document = JsonDocument.Parse(content);

            //With `out` we are telling it to place the object inside the `priceElement`. 
            //But here I made it say that an error should not be thrown if object C is not found or is 0.
            if (!document.RootElement.TryGetProperty("c",out var priceElement)||priceElement.GetDecimal()==0)
            {
                _logger.LogWarning("Finnhub returned zero or missing price for {Symbol}. Response: {Content}", symbol, content);
                throw new ExternalApiException($"Invalid or zero price returned for symbol: {symbol}");
            }
            return priceElement.GetDecimal();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Request failed while fetching price for {Symbol}", symbol);
            throw new ExternalApiException($"Failed to connect to Finnhub API for {symbol}", ex);
            
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing failed for {Symbol} price response", symbol);
            throw new ExternalApiException($"Failed to parse price data from Finnhub for {symbol}", ex);
        }
    }
}
