using Hodler.Domain.Shared.Failures;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Failures;

public class NoPortfolioFoundForUserFailure : Failure
{
    public UserId UserId { get; }

    public NoPortfolioFoundForUserFailure(UserId userId)
    {
        UserId = userId;
    }
}