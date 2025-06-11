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
        IManualTransactions manualTransactions,
        UserId userId,
        IBitcoinWallets? bitcoinWallets = null
    )
    {
        ArgumentNullException.ThrowIfNull(manualTransactions);
        ArgumentNullException.ThrowIfNull(userId);

        ManualTransactions = manualTransactions;
        BitcoinWallets = bitcoinWallets ?? new BitcoinWallets.BitcoinWallets([]);
        UserId = userId;
        Id = portfolioId;
    }

    public PortfolioId Id { get; }
    public UserId UserId { get; }
    public IManualTransactions ManualTransactions { get; private set; }
    public IBitcoinWallets BitcoinWallets { get; private set; }


    public SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        var syncResult = ManualTransactions.Sync(transactions.ToList());

        if (syncResult.Changed)
            ManualTransactions = syncResult.CurrentState;

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

        if (ManualTransactions.IsNullOrEmpty())
        {
            return
            [
                new ChartSpot(today.AddDays(-1), FiatAmount.Zero(userDisplayCurrency)),
                new ChartSpot(today, FiatAmount.Zero(userDisplayCurrency))
            ];
        }

        var startDate = ManualTransactions
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
        ManualTransactions.GetSummaryReportAsync(currentBitcoinPriceProvider, cancellationToken);

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

        if (ManualTransactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));

        ManualTransactions = new ManualTransactions(ManualTransactions.Append(newTransaction));

        return new SuccessResult();
    }

    public IResult RemoveTransaction(TransactionId transactionId)
    {
        var transaction = ManualTransactions
            .AsValueEnumerable()
            .FirstOrDefault(x => x.Id == transactionId);

        if (transaction is null)
            return new SuccessResult();

        ManualTransactions = ManualTransactions.Remove(transactionId);

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

        var transaction = ManualTransactions
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

        if (ManualTransactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));

        ManualTransactions = new ManualTransactions(ManualTransactions.Remove(transactionId).Append(newTransaction));

        return new SuccessResult();
    }

    public async Task<IResult> ConnectBitcoinWallet(
        BitcoinAddress address,
        WalletName walletName,
        IBitcoinBlockchainService blockchainService,
        WalletAvatar? avatar = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(walletName);

        if (BitcoinWallets.AlreadyConnected(address))
            return new FailureResult(new BitcoinWalletAlreadyExistsFailure(address));

        BitcoinWallets = await BitcoinWallets.ConnectWalletAsync(
            Id,
            address,
            walletName,
            blockchainService,
            avatar,
            cancellationToken
        );

        return new SuccessResult();
    }


    public IResult DisconnectBitcoinWallet(BitcoinWalletId walletId)
    {
        ArgumentNullException.ThrowIfNull(walletId);

        if (!BitcoinWallets.AlreadyConnected(walletId))
            return new FailureResult(new BitcoinWalletIsNotConnectedFailure(walletId));

        BitcoinWallets = BitcoinWallets.Disconnect(walletId);

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

        var wallet = BitcoinWallets.FindById(walletId);

        if (wallet == null)
            return new SyncResult<IPortfolio>(false, this);

        try
        {
            BitcoinWallets = await BitcoinWallets.SyncWalletAsync(walletId, blockchainService, cancellationToken);

            return new SyncResult<IPortfolio>(true, this);
        }
        catch (Exception ex)
        {
            return new SyncResult<IPortfolio>(false, this);
        }
    }

    private FiatAmount CalculatePortfolioValueOnDateAsync(IBitcoinPrice btcPriceOnDate)
    {
        var transactionsTillDate = ManualTransactions
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
            new ManualTransactions([]),
            userId
        );
    }
}