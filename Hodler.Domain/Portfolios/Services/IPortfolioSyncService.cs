using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Services;

public interface IPortfolioSyncService
{
    Task<IPortfolio> SyncWithExchangeAsync(UserId userId, CancellationToken cancellationToken);
}