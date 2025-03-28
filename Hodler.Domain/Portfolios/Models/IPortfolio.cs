using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    PortfolioId Id { get; }
    UserId UserId { get; }
    ITransactions Transactions { get; }

    SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions);

    Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    );

    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    );

    IResult AddTransaction(
        TransactionType transactionType,
        DateTime date,
        FiatAmount price,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange
    );
}