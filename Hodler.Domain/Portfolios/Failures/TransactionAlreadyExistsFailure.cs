using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Portfolios.Failures;

public class TransactionAlreadyExistsFailure : Failure
{
    public TransactionId NewTransactionId { get; }

    public TransactionAlreadyExistsFailure(Transaction newTransaction)
    {
        NewTransactionId = newTransaction.Id;
    }
}