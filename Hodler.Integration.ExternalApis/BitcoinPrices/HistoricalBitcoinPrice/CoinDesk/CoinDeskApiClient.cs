using System.Net.Http.Json;
using System.Text.Json;
using Corz.DomainDriven.Abstractions.Exceptions;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared;
using Hodler.Domain.Shared.Models;
using Hodler.Integration.ExternalApis.Failures;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

public class CoinDeskApiClient : ICoinDeskApiClient
{
    // Chunk the request into 2000 blocks to comply with limit
    private const int RateLimit = 2000;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ILogger<CoinDeskApiClient> _logger;

    public CoinDeskApiClient(
        ILogger<CoinDeskApiClient> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieve all available historical data, using limit=2000 and requesting in batches
    /// </summary>
    public async Task<IReadOnlyCollection<IBitcoinPrice>> GetHistoricalDailyBitcoinPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(fiatCurrency);

        try
        {
            var client = CreateClient();
            var rateLimit = startDate == endDate ? 1 : RateLimit;
            var requestsCount = (int)Math.Ceiling((double)(endDate.DayNumber - startDate.DayNumber + 1) / rateLimit);
            var totalSecondsOfDay = (long)TimeSpan.FromDays(1).TotalSeconds;
            var bitcoinPrices = new List<IBitcoinPrice>();
            var endDateInLinuxEpochSeconds = endDate.ToUnixTimeSeconds();

            for (var i = 0; i <= requestsCount; i++)
            {
                var queryString
                    = $"?market=cadli&instrument=BTC-{fiatCurrency.Ticker}&limit={rateLimit}&aggregate=1&fill=true&apply_mapping=true&response_format=JSON&to_ts={endDateInLinuxEpochSeconds}";
                var request = Endpoints.HistoricalDailyBitcoinPrice + queryString;
                var response = await client.GetFromJsonAsync<CoinDeskOhlcvResponse>(request, Converter.Settings, cancellationToken);

                if (response is null || response.Data.Length == 0)
                    continue;

                bitcoinPrices.AddRange(
                    response.Data.Select(x => x.Adapt<BitcoinPrice>())
                );

                endDateInLinuxEpochSeconds = response.Data.Min(x => x.Timestamp)
                                             - totalSecondsOfDay;
            }

            var processedResponse = bitcoinPrices
                .Distinct()
                .OrderBy(x => x.Date)
                .ToList();

            return processedResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while retrieving historical bitcoin prices from CoinDesk API");
            return [];
        }
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient(nameof(CoinDeskApiClient));
        var apiKey = _configuration["ExternalApis:CoinDesk:ApiKey"]!;

        if (string.IsNullOrWhiteSpace(apiKey))
            throw DomainException.CreateFrom(new NoApiKeyProvidedFailure("CoinDesk API"));

        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

        return client;
    }

    private static class Endpoints
    {
        private const string BaseAddress = "https://data-api.coindesk.com/";
        public const string HistoricalDailyBitcoinPrice = BaseAddress + "index/cc/v1/historical/days";
    }

    private static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
        {
            Converters =
            {
                InstrumentConverter.Singleton,
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            },
        };
    }
}