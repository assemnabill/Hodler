using Hodler.Domain.Transactions.Models;

namespace Hodler.Domain.Transactions.Services;

public interface ITransactionsQueryService
{
    Task<TransactionsSummaryReport> GetTransactionsSummaryReportAsync(CancellationToken cancellationToken);
}