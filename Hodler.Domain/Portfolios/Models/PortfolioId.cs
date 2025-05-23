using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Portfolios.Models;

public class PortfolioId : PrimitiveWrapper<Guid, PortfolioId>
{
    public PortfolioId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid {nameof(PortfolioId)}");
    }
}