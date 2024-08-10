using Hodler.Domain.Transactions.Models;

namespace Hodler.Domain.Transactions.Services;

public interface ITransactionParser
{
    ITransactions ParseTransactions(IEnumerable<string[]> lines);
    Transaction? ParseTransaction(string[] line);
}