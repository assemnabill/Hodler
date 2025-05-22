using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public class TransactionId : PrimitiveWrapper<Guid, TransactionId>
{
    public TransactionId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid {nameof(PortfolioId)}");
    }
}