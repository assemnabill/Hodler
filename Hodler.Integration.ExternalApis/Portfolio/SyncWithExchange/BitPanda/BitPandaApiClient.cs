using System.Text;
using System.Text.Json;
using Bitpanda.RestClient;
using Corz.DomainDriven.Abstractions.Exceptions;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using Hodler.Domain.User.Services;
using Hodler.Integration.ExternalApis.Failures;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;

namespace Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.BitPanda;

public class BitPandaApiClient : IBitPandaApiClient
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly IUserSettingsQueryService _userSettingsQueryService;

    public BitPandaApiClient(
        HttpClient httpClient,
        IDistributedCache cache,
        IUserSettingsQueryService userSettingsQueryService
    )
    {
        _userSettingsQueryService = userSettingsQueryService;
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<ITransactions> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var trades = await GetTradesAsync(userId, cancellationToken);
        var transactions = trades
            .Select(x => x.Adapt<TradeAttributes, Transaction>())
            .ToList();

        return new Transactions(transactions);
    }

    private async Task<List<TradeAttributes>> GetTradesAsync(UserId userId, CancellationToken cancellationToken)
    {
        var cachedTrades = await _cache.GetAsync(CacheKey(userId), cancellationToken);

        if (cachedTrades is not null)
        {
            var tradesFromCache = JsonSerializer.Deserialize<TradesResult?>(cachedTrades);
            return RetrieveBitcoinTrades(tradesFromCache);
        }

        var userApiKey = await _userSettingsQueryService.GetBitPandaApiKeyAsync(userId, cancellationToken);

        if (string.IsNullOrWhiteSpace(userApiKey))
            throw DomainException.CreateFrom(new NoApiKeyProvidedFailure("BitPanda"));

        _httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, userApiKey);
        var tradesClient = new TradesClient(_httpClient);
        var tradeResult = await tradesClient.GetAsync(page_size: 100, cancellationToken: cancellationToken);

        CacheTradesAsync(tradeResult, userId, cancellationToken);
        return RetrieveBitcoinTrades(tradeResult);
    }

    private List<TradeAttributes> RetrieveBitcoinTrades(TradesResult? tradesFromCache) => tradesFromCache?.Data
        .Select(x => x.Attributes)
        .Where(x => x.Cryptocoin_id == CryptoCurrency.Bitcoin.Id.ToString())
        .ToList() ?? [];

    private static string CacheKey(UserId userId) => $"{userId}-Trades-BitPanda";

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