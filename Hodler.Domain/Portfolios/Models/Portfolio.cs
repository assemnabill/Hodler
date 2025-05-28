using Corz.Extensions.DateTime;
using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Shared.Results;
using Hodler.Domain.Users.Models;
using Microsoft.Extensions.Internal;
using ZLinq;

namespace Hodler.Domain.Portfolios.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public Portfolio(
        PortfolioId portfolioId,
        ITransactions transactions,
        UserId userId,
        IReadOnlyCollection<IBitcoinWallet>? bitcoinWallets = null
    )
    {
        ArgumentNullException.ThrowIfNull(transactions);
        ArgumentNullException.ThrowIfNull(userId);

        Transactions = transactions;
        BitcoinWallets = bitcoinWallets ?? [];
        UserId = userId;
        Id = portfolioId;
    }

    public PortfolioId Id { get; }
    public UserId UserId { get; }
    public ITransactions Transactions { get; private set; }
    public IReadOnlyCollection<IBitcoinWallet> BitcoinWallets { get; private set; }


    public SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        var syncResult = Transactions.Sync(transactions.ToList());

        if (syncResult.Changed)
            Transactions = syncResult.CurrentState;

        return new SyncResult<IPortfolio>(syncResult.Changed, this);
    }

    public async Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        ISystemClock systemClock,
        CancellationToken cancellationToken = default
    )
    {
        var today = systemClock.UtcNow.ToDate();

        if (Transactions.IsNullOrEmpty())
        {
            return
            [
                new ChartSpot(today.AddDays(-1), FiatAmount.Zero(userDisplayCurrency)),
                new ChartSpot(today, FiatAmount.Zero(userDisplayCurrency))
            ];
        }

        var startDate = Transactions
            .AsValueEnumerable()
            .Select(x => x.Timestamp.ToDate())
            .Min();

        var btcPriceOnDates = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOfDateIntervalAsync(userDisplayCurrency, startDate, today, cancellationToken);

        var chartSpots = btcPriceOnDates
            .AsValueEnumerable()
            .Select(x => new ChartSpot(x.Key, CalculatePortfolioValueOnDateAsync(x.Value)))
            .ToList();

        return chartSpots;
    }

    public Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        FiatCurrency userSettingsCurrency, //todo: consider
        CancellationToken cancellationToken = default
    ) =>
        Transactions.GetSummaryReportAsync(currentBitcoinPriceProvider, cancellationToken);

    public async Task<IResult> AddTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionType transactionType,
        DateTimeOffset timestamp,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        ITransactionSource? newSource,
        CancellationToken cancellationToken = default
    )
    {
        var marketPrice = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOnDateAsync(fiatAmount.FiatCurrency, timestamp.ToDate(), cancellationToken);

        var newTransaction = new Transaction(
            Id,
            new TransactionId(Guid.NewGuid()),
            transactionType,
            fiatAmount,
            bitcoinAmount,
            timestamp,
            marketPrice.Price,
            newSource
        );

        if (Transactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));

        Transactions = new Transactions.Transactions(Transactions.Append(newTransaction));

        return new SuccessResult();
    }

    public IResult RemoveTransaction(TransactionId transactionId)
    {
        var transaction = Transactions
            .AsValueEnumerable()
            .FirstOrDefault(x => x.Id == transactionId);

        if (transaction is null)
            return new SuccessResult();

        Transactions = Transactions.Remove(transactionId);

        return new SuccessResult();
    }

    public async Task<IResult> ModifyTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionId transactionId,
        TransactionType newTransactionType,
        DateTimeOffset newTimestamp,
        FiatAmount newAmount,
        BitcoinAmount newBitcoinAmount,
        ITransactionSource? source,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(historicalBitcoinPriceProvider);
        ArgumentNullException.ThrowIfNull(transactionId);

        var transaction = Transactions
            .AsValueEnumerable()
            .FirstOrDefault(x => x.Id == transactionId);

        if (transaction is null)
            return new FailureResult(new TransactionDoesNotExistFailure(transactionId));

        var marketPrice = transaction.MarketPrice;
        if (newTimestamp.ToDate() != transaction.Timestamp.ToDate())
        {
            marketPrice = (await historicalBitcoinPriceProvider
                    .GetHistoricalPriceOnDateAsync(newAmount.FiatCurrency, newTimestamp.ToDate(), cancellationToken))
                .Price;
        }

        var newTransaction = transaction with
        {
            Type = newTransactionType,
            FiatAmount = newAmount,
            BtcAmount = newBitcoinAmount,
            Timestamp = newTimestamp,
            MarketPrice = marketPrice,
            TransactionSource = source ?? transaction.TransactionSource
        };

        if (Transactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));

        Transactions = new Transactions.Transactions(Transactions.Remove(transactionId).Append(newTransaction));

        return new SuccessResult();
    }

    public IResult ConnectBitcoinWallet(
        BitcoinAddress address,
        string walletName,
        BlockchainNetwork network
    )
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(walletName);
        ArgumentNullException.ThrowIfNull(network);

        if (BitcoinWallets.AsValueEnumerable().Any(w => w.Address.Value == address.Value))
            return new FailureResult(new BitcoinWalletAlreadyExistsFailure(address));

        var newWallet = BitcoinWallet.Create(Id, address, walletName, network);
        BitcoinWallets = BitcoinWallets.Append(newWallet).ToList();

        return new SuccessResult();
    }


    public IResult DisconnectBitcoinWallet(BitcoinWalletId walletId)
    {
        ArgumentNullException.ThrowIfNull(walletId);

        var wallet = BitcoinWallets
            .AsValueEnumerable()
            .FirstOrDefault(w => w.Id == walletId);

        if (wallet == null)
            return new FailureResult(new BitcoinWalletIsNotConnectedFailure(walletId));

        BitcoinWallets = BitcoinWallets
            .AsValueEnumerable()
            .Where(w => w.Id != walletId)
            .ToList();

        return new SuccessResult();
    }

    public async Task<SyncResult<IPortfolio>> SyncBitcoinWalletAsync(
        BitcoinWalletId walletId,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(walletId);
        ArgumentNullException.ThrowIfNull(blockchainService);

        var wallet = BitcoinWallets
            .AsValueEnumerable()
            .FirstOrDefault(w => w.Id == walletId);

        if (wallet == null)
            return new SyncResult<IPortfolio>(false, this);

        try
        {
            var walletTransactions = await blockchainService
                .GetTransactionsAsync(wallet, cancellationToken);

            var syncResult = Transactions.Sync(walletTransactions);
            if (syncResult.Changed)
                Transactions = syncResult.CurrentState;

            var updatedWallet = await wallet.UpdateBalanceAsync(blockchainService, cancellationToken);

            BitcoinWallets = BitcoinWallets
                .AsValueEnumerable()
                .Select(w => w.Id == walletId ? updatedWallet : w)
                .ToList();

            return new SyncResult<IPortfolio>(true, this);
        }
        catch (Exception ex)
        {
            return new SyncResult<IPortfolio>(false, this);
        }
    }

    private FiatAmount CalculatePortfolioValueOnDateAsync(IBitcoinPrice btcPriceOnDate)
    {
        var transactionsTillDate = Transactions
            .AsValueEnumerable()
            .Where(x => x.Timestamp.ToDate() <= btcPriceOnDate.Date)
            .OrderBy(x => x.Timestamp)
            .ToList();

        if (transactionsTillDate.Count == 0)
            return new FiatAmount(0, btcPriceOnDate.Currency);

        var netBtcOnDate = transactionsTillDate
            .Sum(x => x.Type == TransactionType.Buy ? x.BtcAmount : -x.BtcAmount);

        return new FiatAmount(netBtcOnDate * btcPriceOnDate.Price, btcPriceOnDate.Currency);
    }

    public static IPortfolio CreateNew(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new Portfolio(
            new PortfolioId(Guid.NewGuid()),
            new Transactions.Transactions([]),
            userId
        );
    }
}