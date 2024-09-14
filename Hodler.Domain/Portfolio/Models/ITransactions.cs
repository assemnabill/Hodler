using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<TransactionsSummaryReport> GetSummaryReportAsync(
        ICurrentPriceProvider currentPriceProvider,
        CancellationToken cancellationToken);

    SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions);
}