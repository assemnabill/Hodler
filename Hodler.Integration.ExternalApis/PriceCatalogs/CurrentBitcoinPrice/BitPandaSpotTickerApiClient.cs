using System.Net.Http.Json;
using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.CurrentBitcoinPrice;

public class BitPandaSpotTickerApiClient : IBitPandaTickerApiClient
{
    private readonly HttpClient _httpClient;

    public BitPandaSpotTickerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken)
    {
        const string tickerUri = "https://api.bitpanda.com/v1/ticker";
        var response = await _httpClient
            .GetFromJsonAsync<Dictionary<string, Dictionary<string, decimal>>>(tickerUri, cancellationToken)!;

        if (!response.TryGetValue(CryptoCurrency.Bitcoin.Symbol, out var bitcoinPrice))
        {
            throw new ApplicationException("Bitcoin price not found in API response.");
        }

        var bitcoinPriceCatalog = new FiatAmountCatalog(
            IFiatAmountCatalog.SupportedFiatCurrencies
                .Select(fiatCurrency => new FiatAmount(bitcoinPrice[fiatCurrency.Ticker], fiatCurrency))
                .ToList()
        );

        return bitcoinPriceCatalog;
    }
}