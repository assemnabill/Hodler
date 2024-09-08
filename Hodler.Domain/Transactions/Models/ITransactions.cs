using Hodler.Domain.Transactions.Services;

namespace Hodler.Domain.Transactions.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<TransactionsSummaryReport> GetSummaryReportAsync(
        ICurrentPriceProvider currentPriceProvider,
        CancellationToken cancellationToken);
}