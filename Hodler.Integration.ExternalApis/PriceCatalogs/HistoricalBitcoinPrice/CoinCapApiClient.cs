using System.Net.Http.Json;
using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBitcoinPrice;

public class CoinCapApiClient : ICoinCapApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    // todo: usd price see docs: https://docs.coincap.io/#61e708a8-8876-4fb2-a418-86f12f308978

    public CoinCapApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CoinCapCandlesResponse> GetCandlesAsync(
        FiatCurrency baseAsset,
        CoinCapCandlesInterval interval,
        long startInUnixMilliseconds,
        long endInUnixMilliseconds,
        CancellationToken cancellationToken = default
    )
    {
        // api.coincap.io/v2/assets/bitcoin/history?interval=d1
        // TODO: IMPLEMENT AND TEST

        var client = _httpClientFactory.CreateClient("CoinCapClient");

        client.BaseAddress = new Uri("api.coincap.io/v2");
        var result = await client.GetFromJsonAsync<CoinCapCandlesResponse>(
            "api.coincap.io/v2/assets/bitcoin/history?interval=d1",
            cancellationToken: cancellationToken
        );


        return result;
    }
}