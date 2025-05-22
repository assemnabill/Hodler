using System.Net.Http.Json;
using System.Text.Json;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using BitcoinAddress = Hodler.Domain.Portfolios.Models.BitcoinWallets.BitcoinAddress;
using Transaction = Hodler.Domain.Portfolios.Models.Transactions.Transaction;

namespace Hodler.Application.Portfolios.Services;

public class BitcoinBlockchainService : IBitcoinBlockchainService
{
    private static JsonSerializerOptions _jsonOptions = null!;
    private readonly IDistributedCache _cache;
    private readonly HttpClient _httpClient;
    private readonly ILogger<BitcoinBlockchainService> _logger;

    public BitcoinBlockchainService(
        HttpClient httpClient,
        IDistributedCache cache,
        ILogger<BitcoinBlockchainService> logger
    )
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://blockstream.info/api/");
        _cache = cache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions { WriteIndented = false };
    }

    public async Task<List<Transaction>> GetTransactionsAsync(IBitcoinWallet wallet, CancellationToken cancellationToken = default)
    {
        var address = wallet.Address;
        try
        {
            var cacheKey = $"btc-txs-{address.Value}";

            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrWhiteSpace(cachedData))
            {
                var cachedTxs = JsonSerializer.Deserialize<List<Transaction>>(cachedData, _jsonOptions);
                return cachedTxs ?? [];
            }

            // First, get all transaction IDs for the address
            var transactionIds = await _httpClient.GetFromJsonAsync<string[]>(
                $"address/{address.Value}/txs",
                cancellationToken: cancellationToken);

            if (transactionIds == null || transactionIds.Length == 0)
                return [];

            // Fetch details for each transaction (parallel with limit)
            var transactions = new List<BlockchainTransaction>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = 3 };

            await Parallel
                .ForEachAsync(transactionIds, options,
                    async (txId, ct) =>
                    {
                        var tx = await GetTransactionDetails(txId, address.Value, ct);

                        if (tx != null)
                            transactions.Add(tx);
                    });

            var result = transactions
                .OrderByDescending(t => t.Timestamp)
                .Select(x => x.Adapt<Transaction>())
                .ToList();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result, _jsonOptions),
                cacheOptions,
                cancellationToken
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions for {Address}", address.Value);
            return [];
        }
    }

    public async Task<BitcoinAmount> GetCurrentBalanceAsync(BitcoinAddress address, CancellationToken cancellationToken = default)
    {
        try
        {
            var balance = await _httpClient.GetFromJsonAsync<decimal>(
                $"address/{address.Value}/balance",
                cancellationToken: cancellationToken);

            return BitcoinAmount.FromSatoshis(balance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching balance for {Address}", address.Value);
            throw;
        }
    }

    private async Task<BlockchainTransaction?> GetTransactionDetails(
        string txId,
        string address,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _httpClient.GetFromJsonAsync<BlockchainApiTransaction>(
            $"tx/{txId}", cancellationToken: cancellationToken);

        // Find our address in the outputs to determine amount
        var relevantOutput = transaction?.Vout
            .FirstOrDefault(o => o.ScriptpubkeyAddress == address);

        if (relevantOutput == null)
            return null;

        return new BlockchainTransaction(
            Amount: BitcoinAmount.FromSatoshis(relevantOutput.Value),
            TransactionHash: transaction?.Txid ?? txId,
            Timestamp: DateTimeOffset.FromUnixTimeSeconds(transaction.Status.BlockTime),
            FiatValue: 0, // Will be filled later using historical price data
            Price: 0
        );
    }

    private record BlockchainApiTransaction(
        string Txid,
        List<TransactionOutput> Vout,
        TransactionStatus Status
    );

    private record TransactionOutput(
        string ScriptpubkeyAddress,
        decimal Value
    );

    private record TransactionStatus(long BlockTime);
}