using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.DomainDriven.Abstractions.Models.Results;
using Corz.Extensions.DateTime;
using Hodler.Domain.CryptoExchanges.Models;
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

    public PortfolioId Id { get; }
    public UserId UserId { get; }
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

    public async Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Fill gap dates between transactions with => btcPriceOnDate * btcHoldingsOnDate
        var candels = new List<ChartSpot>();

        foreach (var transaction in Transactions)
        {
            var dateOfTransaction = transaction.Timestamp.ToDate();
            var portfolioValueOnDate = await Transactions.GetPortfolioValueOnDateAsync(
                dateOfTransaction,
                historicalBitcoinPriceProvider,
                userDisplayCurrency,
                cancellationToken
            );

            candels.Add(new ChartSpot(dateOfTransaction, portfolioValueOnDate));
        }

        return candels;
    }

    public Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
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


    public static IPortfolio CreateNew(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new Portfolio(new PortfolioId(Guid.NewGuid()), new Transactions([]), userId);
    }
}