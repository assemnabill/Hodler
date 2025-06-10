using System.Net.Http.Json;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.Logging;
using BitcoinAddress = Hodler.Domain.Portfolios.Models.BitcoinWallets.BitcoinAddress;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BitcoinBlockchainService : IBitcoinBlockchainService
{
    private const string BlockchainInfoUrl = "https://blockchain.info";
    private const string BlockstreamUrl = "https://blockstream.info/api";
    private readonly HttpClient _httpClient;
    private readonly ILogger<BitcoinBlockchainService> _logger;

    public BitcoinBlockchainService(
        HttpClient httpClient,
        ILogger<BitcoinBlockchainService> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<BlockchainTransaction>> GetTransactionsAsync(
        IBitcoinWallet wallet,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var requestUri = $"{BlockstreamUrl}/address/{wallet.Address.Value}/txs";
            var response = await _httpClient.GetFromJsonAsync<BlockstreamTransaction[]>(requestUri, cancellationToken);

            if (response == null)
                return [];

            // todo: replace with historical price data
            var btcPrice = await GetCurrentBitcoinPriceAsync(cancellationToken);

            var transactions = RetrieveBlockchainTransactions(wallet, response, btcPrice);

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
            var response = await _httpClient.GetFromJsonAsync<BlockstreamAddressReponse>(requestUri, cancellationToken);

            if (response == null || response.ChainStats == null)
            {
                _logger.LogError("Incomplete or null response received for address {Address}", walletAddress);
                return BitcoinAmount.Zero;
            }

            var netSats = (decimal)(response.ChainStats.FundedTxoSum - response.ChainStats.SpentTxoSum);
            var balance = BitcoinAmount.FromSatoshis(netSats);
            return balance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch balance for address {Address}", walletAddress);
            throw;
        }
    }

    private static List<BlockchainTransaction> RetrieveBlockchainTransactions(
        IBitcoinWallet wallet,
        BlockstreamTransaction[] response,
        FiatAmount btcPrice
    ) =>
        response
            .Select(tx =>
            {
                var bitcoinAmount = BitcoinAmount.FromSatoshis(tx.Outputs
                    .Where(o => o.ScriptPubKeyAddress == wallet.Address.Value)
                    .Sum(o => o.Value));

                var networkFee = BitcoinAmount.FromSatoshis(tx.Fee);

                var transactionType = tx.Outputs.Any(o => o.ScriptPubKeyAddress == wallet.Address.Value)
                    ? TransactionType.Received
                    : TransactionType.Sent;

                // Get most relevant FROM address (exclude change addresses)
                var fromAddress = tx.Inputs
                    .Where(i => i.PrevOut.ScriptPubKeyAddress != wallet.Address.Value)
                    .Select(i => i.PrevOut.ScriptPubKeyAddress)
                    .FirstOrDefault() ?? string.Empty;

                // Get most relevant TO address (exclude change addresses)
                var toAddress = tx.Outputs
                    .Where(o => o.ScriptPubKeyAddress != wallet.Address.Value)
                    .Select(o => o.ScriptPubKeyAddress)
                    .FirstOrDefault() ?? string.Empty;

                // For receives, if all outputs are to us, show the first input
                if (transactionType == TransactionType.Received && string.IsNullOrWhiteSpace(toAddress))
                {
                    toAddress = wallet.Address.Value;
                    fromAddress = tx.Inputs.FirstOrDefault()?.PrevOut.ScriptPubKeyAddress ?? "Unknown";
                }

                // For sends, if all inputs are from us, show the first output
                if (transactionType != TransactionType.Received && string.IsNullOrWhiteSpace(fromAddress))
                {
                    fromAddress = wallet.Address.Value;
                    toAddress = tx.Outputs.FirstOrDefault()?.ScriptPubKeyAddress ?? "Unknown";
                }

                var blockchainTransactionStatus = tx.Status.Confirmed ? BlockchainTransactionStatus.Confirmed : BlockchainTransactionStatus.Pending;

                return new BlockchainTransaction(
                    Amount: bitcoinAmount,
                    TransactionHash: new TransactionHash(tx.TxId),
                    MarketPrice: btcPrice,
                    FiatValue: bitcoinAmount.Amount * btcPrice,
                    Timestamp: DateTimeOffset.FromUnixTimeSeconds(tx.Status.BlockTime),
                    Status: blockchainTransactionStatus,
                    FromAddress: new BitcoinAddress(fromAddress),
                    ToAddress: new BitcoinAddress(toAddress),
                    NetworkFee: networkFee,
                    FiatFee: networkFee * btcPrice,
                    Note: null,
                    TransactionType: transactionType,
                    PortfolioId: wallet.PortfolioId,
                    BitcoinWalletId: wallet.Id
                );
            })
            .ToList();

    public async Task<FiatAmount> GetCurrentBitcoinPriceAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BlockchainInfoTicker>(
                $"{BlockchainInfoUrl}/ticker",
                cancellationToken);

            return new FiatAmount(response?.USD?.Last ?? 0, FiatCurrency.UsDollar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch current Bitcoin price");
            return FiatAmount.ZeroUsDollars;
        }
    }
}