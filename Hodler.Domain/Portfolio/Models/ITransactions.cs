using Hodler.Domain.Portfolio.Services;

namespace Hodler.Domain.Portfolio.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<TransactionsSummaryReport> GetSummaryReportAsync(
        ICurrentPriceProvider currentPriceProvider,
        CancellationToken cancellationToken);

    ITransactions Sync(IEnumerable<Transaction> transactions);
}