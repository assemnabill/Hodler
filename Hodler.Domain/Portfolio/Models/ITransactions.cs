using Hodler.Domain.PriceCatalog.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<PortfolioSummary> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    );

    SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions);
}