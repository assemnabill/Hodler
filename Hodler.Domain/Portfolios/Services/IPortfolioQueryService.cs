using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Services;

public interface IPortfolioQueryService   
{
    Task<IPortfolio> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);

    Task<PortfolioSummaryInfo> GetPortfolioSummaryAsync(UserId userId, CancellationToken cancellationToken);
}