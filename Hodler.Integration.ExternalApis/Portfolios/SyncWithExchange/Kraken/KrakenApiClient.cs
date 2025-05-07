using System.Text;
using System.Text.Json;
using Corz.DomainDriven.Abstractions.Exceptions;
using Corz.Extensions.DateTime;
using Corz.Extensions.Enumeration;
using CryptoExchange.Net.Authentication;
using Hodler.Domain.BitcoinPrices.Failures;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Services;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.Kraken;

public class KrakenApiClient : IKrakenApiClient
{
    private readonly IDistributedCache _cache;
    private readonly IKrakenRestClient _krakenRestClient;
    private readonly IUserSettingsQueryService _userSettingsQueryService;

    public KrakenApiClient(
        IDistributedCache cache,
        IUserSettingsQueryService userSettingsQueryService,
        IKrakenRestClient krakenRestClient
    )
    {
        _userSettingsQueryService = userSettingsQueryService;
        _krakenRestClient = krakenRestClient;
        _cache = cache;
    }

    public async Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(
        UserId userId,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var cachedTrades = await _cache.GetAsync(CacheKey(userId), cancellationToken);

        if (cachedTrades is not null)
        {
            var tradesFromCache = JsonSerializer.Deserialize<List<TransactionInfo>>(cachedTrades);
            return tradesFromCache!;
        }

        await InitKrakenApiCredentialsAsync(userId, cancellationToken);
        var ledgerEntries = await LoadKrakenLedgerEntriesAsync(cancellationToken);
        var transactionInfos = ProcessLedgerInfoEntries(ledgerEntries);

        CacheTradesAsync(transactionInfos, userId, cancellationToken);

        return transactionInfos;
    }

    private async Task<List<KrakenLedgerEntry>> LoadKrakenLedgerEntriesAsync(CancellationToken cancellationToken)
    {
        const int pageSize = 50;
        var ledgerInfo = new List<KrakenLedgerEntry>();
        var pages = 1;

        for (var page = 0; page < pages; page++)
        {
            var ledgerInfoPage = await _krakenRestClient.SpotApi.Account
                .GetLedgerInfoAsync(resultOffset: page * pageSize, ct: cancellationToken);

            if (!ledgerInfoPage.Success)
                throw new ApplicationException(ledgerInfoPage.Error?.Message ?? "Unknown error");

            if (page == 0)
                pages = Convert.ToInt32(Math.Ceiling(ledgerInfoPage.Data.Count / (double)pageSize));

            var ledgerEntries = ledgerInfoPage.Data.Ledger
                .Select(x => x.Value);

            ledgerInfo.AddRange(ledgerEntries);
        }

        return ledgerInfo;
    }

    // TODO: TEST if it is still working after refactoring
    private async Task InitKrakenApiCredentialsAsync(UserId userId, CancellationToken cancellationToken)
    {
        var apiKey = await _userSettingsQueryService.GetApiKeyAsync(userId, ApiKeyName.Kraken, cancellationToken);

        if (apiKey is null)
            throw DomainException.CreateFrom(new NoApiKeyProvidedFailure(ApiKeyName.Kraken.GetDescription()));

        var apiCredentials = new ApiCredentials(apiKey.Value, apiKey.Secret!);

        _krakenRestClient.SetApiCredentials(apiCredentials);
    }

    private List<TransactionInfo> ProcessLedgerInfoEntries(List<KrakenLedgerEntry> ledgerEntries)
    {
        var transactions = ledgerEntries
            .Where(x => x.Type is LedgerEntryType.Spend or LedgerEntryType.Receive)
            .GroupBy(x => x.ReferenceId)
            .Select(x =>
            {
                var spendingEntry = x.FirstOrDefault(y => y.Type is LedgerEntryType.Spend);
                var receivingEntry = x.FirstOrDefault(y => y.Type is LedgerEntryType.Receive);

                if (IsInvalidBitcoinTransaction(spendingEntry, receivingEntry))
                    return null;

                var transactionType = receivingEntry!.Asset.Contains("XBT")
                    ? TransactionType.Buy
                    : TransactionType.Sell;

                FiatAmount marketPrice;
                FiatAmount fiatAmount;
                BitcoinAmount bitcoinAmount;

                if (transactionType == TransactionType.Buy)
                {
                    marketPrice = new FiatAmount(Math.Abs(spendingEntry!.Quantity / receivingEntry.Quantity),
                        GetFiatCurrencyByTicker(spendingEntry));
                    fiatAmount = new FiatAmount(spendingEntry.Quantity, GetFiatCurrencyByTicker(spendingEntry));
                    bitcoinAmount = new BitcoinAmount(receivingEntry.Quantity);
                }
                else
                {
                    marketPrice = new FiatAmount(Math.Abs(receivingEntry.Quantity / spendingEntry!.Quantity),
                        GetFiatCurrencyByTicker(receivingEntry));
                    fiatAmount = new FiatAmount(receivingEntry.Quantity, GetFiatCurrencyByTicker(receivingEntry));
                    bitcoinAmount = new BitcoinAmount(spendingEntry.Quantity);
                }

                return new TransactionInfo(
                    new TransactionId(Guid.NewGuid()),
                    transactionType,
                    fiatAmount,
                    bitcoinAmount,
                    marketPrice,
                    spendingEntry.Timestamp.ToDateTimeOffset().ToUniversalTime(),
                    CryptoExchangeName.Kraken
                );
            });

        return transactions
            .Where(x => x != null)
            .ToList()!;
    }

    private static FiatCurrency GetFiatCurrencyByTicker(KrakenLedgerEntry receivingEntry)
    {
        return FiatCurrency.GetByTicker(receivingEntry.Asset.Substring(0, 3));
    }

    private static bool IsInvalidBitcoinTransaction(KrakenLedgerEntry? spendingEntry, KrakenLedgerEntry? receivingEntry) =>
        spendingEntry is null
        || receivingEntry is null
        || (!spendingEntry.Asset.Contains("XBT") && !receivingEntry.Asset.Contains("XBT"));

    private static string CacheKey(UserId userId) => $"{userId}-Trades-{CryptoExchangeName.Kraken.GetDescription()}";

    private async Task CacheTradesAsync(
        List<TransactionInfo> transactionInfos,
        UserId userId,
        CancellationToken cancellationToken
    )
    {
        var cachingLifeDuration = TimeSpan.FromMinutes(60);
        var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = cachingLifeDuration, };
        var transactionInfoBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(transactionInfos));

        await _cache.SetAsync(CacheKey(userId), transactionInfoBytes, cacheOptions, cancellationToken);
    }
}