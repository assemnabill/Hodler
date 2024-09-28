using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Contracts.Shared;

namespace Hodler.Web;

public class HodlerApiClient(HttpClient httpClient)
{
    public async Task<PortfolioSummaryDto?> GetPortfolioSummaryAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<PortfolioSummaryDto>(
            $"api/Portfolio/{userId}/summary",
            cancellationToken);
    }

    public async Task<PortfolioInfoDto?> GetPortfolioInfoAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<PortfolioInfoDto>(
            $"api/Portfolio/{userId}",
            cancellationToken);
    }

    public async Task<PortfolioInfoDto?> SyncWithExchangeAsync(
        CryptoExchangeNames exchangeNamesName,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var result = await httpClient.PostAsJsonAsync(
            $"api/Portfolio/sync/{(int)exchangeNamesName}",
            userId,
            cancellationToken);

        var portfolioInfo = result.IsSuccessStatusCode
            ? await result.Content.ReadFromJsonAsync<PortfolioInfoDto>(cancellationToken)
            : null;
        
        return portfolioInfo;
    }
}