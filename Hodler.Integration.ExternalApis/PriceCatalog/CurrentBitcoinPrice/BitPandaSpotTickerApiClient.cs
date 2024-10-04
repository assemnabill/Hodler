using System.Net.Http.Json;
using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.PriceCatalog.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalog.CurrentBitcoinPrice;

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
            SupportedFiatCurrencies
                .Select(fiatCurrency => new FiatAmount(bitcoinPrice[fiatCurrency.Ticker], fiatCurrency))
                .ToList()
        );

        return bitcoinPriceCatalog;
    }

    private static IReadOnlyCollection<FiatCurrency> SupportedFiatCurrencies
    {
        get
        {
            IReadOnlyCollection<FiatCurrency> supportedFiatCurrencies =
            [
                FiatCurrency.Euro,
                FiatCurrency.UsDollar,
                FiatCurrency.SwissFranc,
                FiatCurrency.BritishPound,
                FiatCurrency.TurkishLira,
                FiatCurrency.PolishZloty,
                FiatCurrency.HungarianForint,
                FiatCurrency.CzechKoruna,
                FiatCurrency.SwedishKrona,
                FiatCurrency.DanishKrone
            ];
            return supportedFiatCurrencies;
        }
    }
}