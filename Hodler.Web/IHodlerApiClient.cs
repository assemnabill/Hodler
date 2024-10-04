using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Contracts.Shared;

namespace Hodler.Web;

public interface IHodlerApiClient
{
    string BaseUri { get; }
    
    Task<PortfolioInfoDto?> SyncWithExchangeAsync(
        CryptoExchangeNames exchangeNamesName,
        string userId,
        CancellationToken cancellationToken = default);

    Task<PortfolioInfoDto?> GetPortfolioInfoAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<PortfolioSummaryDto?> GetPortfolioSummaryAsync(
        string userId,
        CancellationToken cancellationToken = default);

}