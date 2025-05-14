using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.DomainDriven.Abstractions.Models.Results;
using Corz.Extensions.DateTime;
using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Microsoft.Extensions.Internal;

namespace Hodler.Domain.Portfolios.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public PortfolioId Id { get; }
    public UserId UserId { get; }
    public ITransactions Transactions { get; private set; }

    public Portfolio(PortfolioId portfolioId, ITransactions transactions, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        Transactions = transactions;
        UserId = userId;
        Id = portfolioId;
    }

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
            .Select(x => x.Timestamp.ToDate())
            .Min();

        var btcPriceOnDates = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOfDateIntervalAsync(userDisplayCurrency, startDate, today, cancellationToken);

        var chartSpots = btcPriceOnDates
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
        DateTime date,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange,
        CancellationToken cancellationToken = default
    )
    {
        var marketPrice = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOnDateAsync(fiatAmount.FiatCurrency, date.ToDate(), cancellationToken);

        var newTransaction = new Transaction(
            Id,
            new TransactionId(Guid.NewGuid()),
            transactionType,
            fiatAmount,
            bitcoinAmount,
            date,
            marketPrice.Price,
            cryptoExchange
        );

        if (Transactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));


        Transactions = new Transactions(Transactions.Append(newTransaction));

        return new SuccessResult();
    }

    public IResult RemoveTransaction(TransactionId transactionId)
    {
        var transaction = Transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction is null)
            return new SuccessResult();

        Transactions = Transactions.Remove(transactionId);

        return new SuccessResult();
    }

    public async Task<IResult> ModifyTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionId transactionId,
        TransactionType newTransactionType,
        DateTime newDate,
        FiatAmount newAmount,
        BitcoinAmount newBitcoinAmount,
        CryptoExchangeName? newCryptoExchange,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(historicalBitcoinPriceProvider);
        ArgumentNullException.ThrowIfNull(transactionId);

        var transaction = Transactions.FirstOrDefault(x => x.Id == transactionId);
        if (transaction is null)
            return new FailureResult(new TransactionDoesNotExistFailure(transactionId));

        var marketPrice = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOnDateAsync(newAmount.FiatCurrency, newDate.ToDate(), cancellationToken);

        var newTransaction = transaction with
        {
            Type = newTransactionType,
            FiatAmount = newAmount,
            BtcAmount = newBitcoinAmount,
            Timestamp = newDate,
            MarketPrice = marketPrice.Price,
            CryptoExchange = newCryptoExchange
        };

        if (Transactions.AlreadyExists(newTransaction))
            return new FailureResult(new TransactionAlreadyExistsFailure(newTransaction));

        Transactions = new Transactions(Transactions.Remove(transactionId).Append(newTransaction));

        return new SuccessResult();
    }

    private FiatAmount CalculatePortfolioValueOnDateAsync(IBitcoinPrice btcPriceOnDate)
    {
        var transactionsTillDate = Transactions
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

        return new Portfolio(new PortfolioId(Guid.NewGuid()), new Transactions([]), userId);
    }
}