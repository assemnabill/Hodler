using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBitcoinPrice;

public interface ICoinCapApiClient
{
    Task<CoinCapCandlesResponse> GetCandlesAsync(
        FiatCurrency baseAsset,
        CoinCapCandlesInterval interval,
        long startInUnixMilliseconds,
        long endInUnixMilliseconds,
        CancellationToken cancellationToken = default
    );
}