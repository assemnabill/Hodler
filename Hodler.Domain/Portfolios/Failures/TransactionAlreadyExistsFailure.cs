using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Portfolios.Failures;

public class TransactionAlreadyExistsFailure : Failure
{
    public Transaction NewTransaction { get; }

    public TransactionAlreadyExistsFailure(Transaction newTransaction)
    {
        NewTransaction = newTransaction;
    }
}