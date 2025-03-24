using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBticoinPrice;

public interface ICoinCapApiClient
{
    // todo: usd price see docs: https://docs.coincap.io/#61e708a8-8876-4fb2-a418-86f12f308978
    Task<CoinCapCandlesResponse> GetCandlesAsync(
        FiatCurrency baseAsset,
        CoinCapCandlesInterval interval,
        long startInUnixMilliseconds,
        long endInUnixMilliseconds,
        CancellationToken cancellationToken = default
    );
}