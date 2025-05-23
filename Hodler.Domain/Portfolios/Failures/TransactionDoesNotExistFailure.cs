using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Portfolios.Failures;

public class TransactionDoesNotExistFailure : Failure
{
    public TransactionId TransactionId { get; }

    public TransactionDoesNotExistFailure(TransactionId transactionId)
    {
        TransactionId = transactionId;
    }
}