using Corz.DomainDriven.Abstractions.Failures;
using Hodler.Domain.Portfolios.Models;

namespace Hodler.Domain.Portfolios.Failures;

public class TransactionDoesNotExistFailure : Failure
{
    public TransactionId TransactionId { get; }

    public TransactionDoesNotExistFailure(TransactionId transactionId)
    {
        TransactionId = transactionId;
    }
}