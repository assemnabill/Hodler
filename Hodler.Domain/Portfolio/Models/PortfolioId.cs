using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Models;

public class PortfolioId : PrimitiveWrapper<Guid, UserId>
{
    public PortfolioId(Guid value) : base(value)
    {
        if (value == default)
        {
            throw new ArgumentException("Invalid user id");
        }
    }
}