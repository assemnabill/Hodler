using System.Net.Http.Json;
using System.Text.Json;
using Corz.Extensions.DateTime;
using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.Logging;
using BitcoinAddress = Hodler.Domain.Portfolios.Models.BitcoinWallets.BitcoinAddress;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BitcoinBlockchainService : IBitcoinBlockchainService
{
    private const string BlockstreamUrl = "https://blockstream.info/api";
    private readonly IHistoricalBitcoinPriceProvider _historicalBitcoinPriceProvider;
    private readonly HttpClient _httpClient;
    private readonly ILogger<BitcoinBlockchainService> _logger;

    public BitcoinBlockchainService(
        HttpClient httpClient,
        ILogger<BitcoinBlockchainService> logger,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider
    )
    {
        _httpClient = httpClient;
        _logger = logger;
        _historicalBitcoinPriceProvider = historicalBitcoinPriceProvider;
    }

    public async Task<IReadOnlyCollection<BlockchainTransaction>> GetTransactionsAsync(
        IBitcoinWallet wallet,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new ScriptpubkeyTypeConverter() }
            };

            var requestUri = $"{BlockstreamUrl}/address/{wallet.Address.Value}/txs";
            var response = await _httpClient
                .GetFromJsonAsync<BlockstreamAddressTransactionsResponse[]>(requestUri, options, cancellationToken);

            if (response == null)
                return [];

            var transactions = await RetrieveBlockchainTransactionsAsync(wallet, response, cancellationToken);

            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch transactions for address {Address}", wallet.Address);
            throw;
        }
    }

    public async Task<BitcoinAmount> GetCurrentBalanceAsync(
        BitcoinAddress walletAddress,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var requestUri = $"{BlockstreamUrl}/address/{walletAddress.Value}";
            var response = await _httpClient.GetFromJsonAsync<BlockstreamAddressResponse>(requestUri, cancellationToken);

            var netSats = (decimal)(response?.ChainStatistics.FundedTxoSum - response?.ChainStatistics.SpentTxoSum)!;
            var balance = BitcoinAmount.FromSatoshis(netSats);
            return balance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch balance for address {Address}", walletAddress);
            throw;
        }
    }

    private async Task<List<BlockchainTransaction>> RetrieveBlockchainTransactionsAsync(
        IBitcoinWallet wallet,
        BlockstreamAddressTransactionsResponse[] response,
        CancellationToken cancellationToken
    )
    {

        if (response.IsNullOrEmpty())
            return [];

        var result = new List<BlockchainTransaction>();
        foreach (var tx in response)
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(tx.BitcoinTransactionStatus.BlockTime);

            var btcPriceOnTxDate = await _historicalBitcoinPriceProvider
                .GetHistoricalPriceOnDateAsync(FiatCurrency.UsDollar, timestamp.ToDate(), cancellationToken);

            // Calculate the NET amount for this specific transaction
            var receivedAmount = tx.Vout
                .Where(o => o.ScriptpubkeyAddress == wallet.Address.Value)
                .Sum(o => o.Value);

            var sentAmount = tx.Vin
                .Where(i => i.Prevout.ScriptpubkeyAddress == wallet.Address.Value)
                .Sum(i => i.Prevout.Value);

            var netAmount = receivedAmount - sentAmount;
            var bitcoinAmount = BitcoinAmount.FromSatoshis(netAmount);
            var networkFee = BitcoinAmount.FromSatoshis(tx.Fee);

            var transactionType = tx.Vout.Any(o => o.ScriptpubkeyAddress == wallet.Address.Value)
                ? TransactionType.Received
                : TransactionType.Sent;

            // Get most relevant FROM address (exclude change addresses)
            var fromAddress = tx.Vin
                .Where(i => i.Prevout.ScriptpubkeyAddress != wallet.Address.Value)
                .Select(i => i.Prevout.ScriptpubkeyAddress)
                .FirstOrDefault() ?? string.Empty;

            // Get most relevant TO address (exclude change addresses)
            var toAddress = tx.Vout
                .Where(o => o.ScriptpubkeyAddress != wallet.Address.Value)
                .Select(o => o.ScriptpubkeyAddress)
                .FirstOrDefault() ?? string.Empty;

            // For receives, if all outputs are to us, show the first input
            if (transactionType == TransactionType.Received && string.IsNullOrWhiteSpace(toAddress))
            {
                toAddress = wallet.Address.Value;
                fromAddress = tx.Vin.FirstOrDefault()?.Prevout.ScriptpubkeyAddress;
            }

            // For sends, if all inputs are from us, show the first output
            if (transactionType != TransactionType.Received && string.IsNullOrWhiteSpace(fromAddress))
            {
                fromAddress = wallet.Address.Value;
                toAddress = tx.Vout.FirstOrDefault()?.ScriptpubkeyAddress;
            }

            var blockchainTransactionStatus = tx.BitcoinTransactionStatus.Confirmed
                ? BlockchainTransactionStatus.Confirmed
                : BlockchainTransactionStatus.Pending;

            result.Add(new BlockchainTransaction(
                Amount: bitcoinAmount,
                TransactionHash: new TransactionHash(tx.Txid),
                MarketPrice: btcPriceOnTxDate.Price,
                FiatValue: bitcoinAmount.Amount * btcPriceOnTxDate.Price,
                Timestamp: timestamp,
                Status: blockchainTransactionStatus,
                FromAddress: new BitcoinAddress(fromAddress!),
                ToAddress: new BitcoinAddress(toAddress!),
                NetworkFee: networkFee,
                FiatFee: networkFee * btcPriceOnTxDate.Price,
                Note: null,
                TransactionType: transactionType,
                PortfolioId: wallet.PortfolioId,
                BitcoinWalletId: wallet.Id
            ));
        }

        return result;
    }
}