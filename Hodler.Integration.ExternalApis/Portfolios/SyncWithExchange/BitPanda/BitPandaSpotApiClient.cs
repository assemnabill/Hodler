using System.Text;
using System.Text.Json;
using Bitpanda.RestClient;
using Corz.DomainDriven.Abstractions.Exceptions;
using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Services;
using Hodler.Integration.ExternalApis.Failures;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;

namespace Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.BitPanda;

public class BitPandaSpotApiClient : IBitPandaSpotApiClient
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private readonly IDistributedCache _cache;

    private readonly HttpClient _httpClient;
    private readonly IUserSettingsQueryService _userSettingsQueryService;

    public BitPandaSpotApiClient(
        HttpClient httpClient,
        IDistributedCache cache,
        IUserSettingsQueryService userSettingsQueryService
    )
    {
        _userSettingsQueryService = userSettingsQueryService;
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(
        UserId userId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var trades = await GetTradesAsync(userId, cancellationToken);
        var transactions = trades
            .Select(x => x.Adapt<TradeAttributes, TransactionInfo>())
            .ToList();

        return transactions;
    }

    private async Task<List<TradeAttributes>> GetTradesAsync(UserId userId, CancellationToken cancellationToken)
    {
        var cachedTrades = await _cache.GetAsync(CacheKey(userId), cancellationToken);

        if (cachedTrades is not null)
        {
            var tradesFromCache = JsonSerializer.Deserialize<TradesResult?>(cachedTrades);
            return RetrieveBitcoinTrades(tradesFromCache);
        }

        var userApiKey = await _userSettingsQueryService
            .GetApiKeyAsync(userId, ApiKeyName.BitPanda, cancellationToken);

        if (userApiKey is null)
            throw DomainException.CreateFrom(
                new NoApiKeyProvidedFailure(CryptoExchangeName.BitPanda.GetDescription()));

        _httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, userApiKey.Value);
        var tradesClient = new TradesClient(_httpClient);
        var tradeResult = await tradesClient.GetAsync(page_size: 100, cancellationToken: cancellationToken);

        CacheTradesAsync(tradeResult, userId, cancellationToken);
        return RetrieveBitcoinTrades(tradeResult);
    }

    private List<TradeAttributes> RetrieveBitcoinTrades(TradesResult? tradesFromCache) => tradesFromCache?.Data
        .Select(x => x.Attributes)
        .Where(x => x.Cryptocoin_id == CryptoCurrency.Bitcoin.Id.ToString())
        .ToList() ?? [];

    private static string CacheKey(UserId userId) =>
        $"{userId}-Trades-{CryptoExchangeName.BitPanda.GetDescription()}";

    private async Task CacheTradesAsync(TradesResult tradeResult, UserId userId, CancellationToken cancellationToken)
    {
        var cachingLifeDuration = TimeSpan.FromMinutes(60);

        await _cache.SetAsync(
            CacheKey(userId),
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tradeResult)),
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = cachingLifeDuration,
            },
            cancellationToken);
    }
}