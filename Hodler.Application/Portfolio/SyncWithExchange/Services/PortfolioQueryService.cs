using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.User.Models;

namespace Hodler.Application.Portfolio.SyncWithExchange.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    public Task<IPortfolio> GetByUserIdAsync(UserId userId)
    {
        return Task.FromResult<IPortfolio>(
            new Domain.Portfolio.Models.Portfolio(new Transactions([]), userId)
        );
    }
}