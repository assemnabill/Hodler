using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface ITransactionsQueryService
{
    Task<TransactionsSummaryReport> GetTransactionsSummaryReportAsync(CancellationToken cancellationToken);
    Task<ITransactions> GetTransactionsAsync(CancellationToken cancellationToken);
}