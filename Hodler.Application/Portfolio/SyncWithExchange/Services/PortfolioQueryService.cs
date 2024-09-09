using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Services;

namespace Hodler.Application.Portfolio.SyncWithExchange.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    public Task<IPortfolio> GetByUserIdAsync(Guid userId)
    {
        return Task.FromResult<IPortfolio>(
            new Domain.Portfolio.Models.Portfolio(new Transactions([]))
        );
    }
}