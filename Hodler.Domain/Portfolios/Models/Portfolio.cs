using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.DomainDriven.Abstractions.Models.Results;
using Corz.Extensions.DateTime;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Microsoft.Extensions.Internal;

namespace Hodler.Domain.Portfolios.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public Portfolio(PortfolioId portfolioId, ITransactions transactions, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        Transactions = transactions;
        UserId = userId;
        Id = portfolioId;
    }

    public PortfolioId Id { get; }
    public UserId UserId { get; }
    public ITransactions Transactions { get; private set; }

    public SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        var syncResult = Transactions.Sync(transactions.ToList());

        if (syncResult.Changed)
        {
            Transactions = syncResult.CurrentState;
        }

        return new SyncResult<IPortfolio>(syncResult.Changed, this);
    }

    public async Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        ISystemClock systemClock,
        CancellationToken cancellationToken = default
    )
    {
        var startDate = Transactions
            .Select(x => x.Timestamp.ToDate())
            .Min();

        var endDate = systemClock.UtcNow.ToDate();

        var btcPriceOnDates = await historicalBitcoinPriceProvider
            .GetHistoricalPriceOfDateIntervalAsync(userDisplayCurrency, startDate, endDate, cancellationToken);

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

    public IResult AddTransaction(
        TransactionType transactionType,
        DateTime date,
        FiatAmount price,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange
    )
    {
        Transactions = Transactions
            .Add(Id, transactionType, date, price, bitcoinAmount, cryptoExchange, out var result);

        return result;
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