using Corz.DomainDriven.Abstractions.Failures;
using Hodler.Domain.Portfolios.Models;

namespace Hodler.Domain.Portfolios.Failures;

public class TransactionAlreadyExistsFailure : Failure
{
    public Transaction NewTransaction { get; }

    public TransactionAlreadyExistsFailure(Transaction newTransaction)
    {
        NewTransaction = newTransaction;
    }
}