namespace Hodler.Domain.Transactions.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    TransactionsSummaryReport GetSummaryReport();
}