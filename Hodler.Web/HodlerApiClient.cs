using Hodler.Contracts.Portfolio;
using Hodler.Contracts.Portfolio.PortfolioSummary;

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

    public async Task<PortfolioInfoDto?> GetTransactionsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<PortfolioInfoDto>(
            $"api/Portfolio/{userId}",
            cancellationToken);
    }
}