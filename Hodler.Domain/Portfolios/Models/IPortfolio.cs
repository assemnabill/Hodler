using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Shared.Results;
using Hodler.Domain.Users.Models;
using Microsoft.Extensions.Internal;

namespace Hodler.Domain.Portfolios.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    PortfolioId Id { get; }
    UserId UserId { get; }
    ITransactions Transactions { get; }
    IReadOnlyCollection<IBitcoinWallet> BitcoinWallets { get; }

    SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions);

    Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        ISystemClock systemClock,
        CancellationToken cancellationToken = default
    );

    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        FiatCurrency userSettingsCurrency,
        CancellationToken cancellationToken = default
    );

    Task<IResult> AddTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionType transactionType,
        DateTimeOffset timestamp,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        ITransactionSource cryptoExchange,
        CancellationToken cancellationToken = default
    );

    IResult RemoveTransaction(TransactionId transactionId);

    Task<IResult> ModifyTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionId transactionId,
        TransactionType newTransactionType,
        DateTimeOffset newTimestamp,
        FiatAmount newAmount,
        BitcoinAmount newBitcoinAmount,
        ITransactionSource? source,
        CancellationToken cancellationToken = default
    );

    IResult ConnectBitcoinWallet(
        BitcoinAddress address,
        string walletName,
        BlockchainNetwork network
    );

    IResult DisconnectBitcoinWallet(BitcoinWalletId walletId);

    Task<SyncResult<IPortfolio>> SyncBitcoinWalletAsync(
        BitcoinWalletId walletId,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    );
}