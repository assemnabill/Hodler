using Corz.DomainDriven.Abstractions.Failures;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Failures;

public class PortfolioDoesNotExistFailure : Failure
{
    public UserId UserId { get; }

    public PortfolioDoesNotExistFailure(UserId userId)
    {
        UserId = userId;
    }
}