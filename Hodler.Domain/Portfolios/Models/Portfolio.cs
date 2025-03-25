using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.Extensions.DateTime;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;

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

    public PortfolioId Id { get; private set; }
    public UserId UserId { get; private set; }
    public ITransactions Transactions { get; private set; }

    public SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        var syncResult = Transactions.Sync(transactions);

        if (syncResult.Changed)
        {
            Transactions = syncResult.CurrentState;
        }

        return new SyncResult<IPortfolio>(syncResult.Changed, this);
    }

    public async Task<IReadOnlyCollection<ChartCandle>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Fill gap dates between transactions with => btcPriceOnDate * btcHoldingsOnDate
        var candels = new List<ChartCandle>();

        foreach (var transaction in Transactions)
        {
            var dateOfTransaction = transaction.Timestamp.ToDate();
            var portfolioValueOnDate = await Transactions.GetPortfolioValueOnDateAsync(dateOfTransaction,
                historicalBitcoinPriceProvider, cancellationToken);
            var candle = new ChartCandle(dateOfTransaction, portfolioValueOnDate);
            candels.Add(candle);
        }

        return candels;
    }

    public Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    ) => Transactions.GetSummaryReportAsync(currentBitcoinPriceProvider, cancellationToken);


    public static IPortfolio CreateNew(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new Portfolio(new PortfolioId(Guid.NewGuid()), new Transactions([]), userId);
    }
}