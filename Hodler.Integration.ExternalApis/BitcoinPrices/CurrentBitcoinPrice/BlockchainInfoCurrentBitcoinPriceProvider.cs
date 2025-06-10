using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.CurrentBitcoinPrice;

public class BlockchainInfoCurrentBitcoinPriceProvider : ICurrentBitcoinPriceProvider
{
    private const string BlockchainInfoUrl = "https://blockchain.info";
    private readonly HttpClient _httpClient;
    private readonly ILogger<BlockchainInfoCurrentBitcoinPriceProvider> _logger;

    public BlockchainInfoCurrentBitcoinPriceProvider(
        HttpClient httpClient,
        ILogger<BlockchainInfoCurrentBitcoinPriceProvider> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken)
    {
        var priceCatalog = await GetBitcoinPriceCatalogAsync(cancellationToken);

        return priceCatalog.GetPrice(FiatCurrency.UsDollar);
    }


    public async Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<BlockchainInfoTickerResponse[]>(
            $"{BlockchainInfoUrl}/ticker",
            cancellationToken
        );

        if (response == null || response.Length == 0)
        {
            _logger.LogError("Failed to fetch Bitcoin price catalog from Blockchain.info");
            throw new ApplicationException("Bitcoin price catalog is empty or not available.");
        }

        var bitcoinPrice = response
            .ToDictionary(ticker => ticker.Symbol, ticker => (decimal)ticker.Last);

        var bitcoinPriceCatalog = new FiatAmountCatalog(
            IFiatAmountCatalog.SupportedFiatCurrencies
                .Select(fiatCurrency => new FiatAmount(bitcoinPrice[fiatCurrency.Ticker], fiatCurrency))
                .ToList()
        );

        return bitcoinPriceCatalog;
    }

    private class BlockchainInfoTickerResponse
    {
        [JsonPropertyName("15m")]
        public double The15M { get; set; }

        [JsonPropertyName("last")]
        public double Last { get; set; }

        [JsonPropertyName("buy")]
        public double Buy { get; set; }

        [JsonPropertyName("sell")]
        public double Sell { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}