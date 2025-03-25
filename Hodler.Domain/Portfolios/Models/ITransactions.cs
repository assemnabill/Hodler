using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    );

    SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions);

    Task<FiatAmount> GetPortfolioValueOnDateAsync(
        DateOnly dateOfTransaction,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    );
}