using Bitpanda.RestClient;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.User.Services;
using Mapster;

namespace Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange;

public class BitPandaApiClient : IBitPandaApiClient
{
    // TODO: Validation        & caching

    private const string ApiKeyHeaderName = "X-API-KEY";

    private readonly HttpClient _httpClient;
    private readonly IUserSettingsQueryService _userSettingsQueryService;


    public BitPandaApiClient(HttpClient httpClient, IUserSettingsQueryService userSettingsQueryService)
    {
        _userSettingsQueryService = userSettingsQueryService;
        _httpClient = httpClient;
    }

    public async Task<ITransactions> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var trades = await GetTradesAsync(userId, cancellationToken);
        var transactions = trades
            .Select(x => x.Adapt<TradeAttributes, Transaction>())
            .ToList();

        return new Transactions(transactions);
    }

    private async Task<List<TradeAttributes>> GetTradesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userApiKey = await _userSettingsQueryService.GetBitPandaApiKeyAsync(userId, cancellationToken);
        _httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, userApiKey);
        // TODO: Validation        

        var tradesClient = new TradesClient(_httpClient);
        var tradeResult = await tradesClient.GetAsync(page_size: 100, cancellationToken: cancellationToken);

        var trades = tradeResult.Data
            .Select(x => x.Attributes)
            .Where(x => x.Cryptocoin_id == CryptoCurrency.Bitcoin.Id.ToString())
            .ToList();

        return trades;
    }
}