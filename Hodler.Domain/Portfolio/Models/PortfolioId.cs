using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Portfolio.Models;

public class PortfolioId : PrimitiveWrapper<Guid, PortfolioId>
{
    public PortfolioId(Guid value) : base(value)
    {
        if (value == default)
        {
            throw new ArgumentException($"Invalid {nameof(PortfolioId)}");
        }
    }
}