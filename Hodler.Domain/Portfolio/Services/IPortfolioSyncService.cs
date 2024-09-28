using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface IPortfolioSyncService
{
    Task<IPortfolio> SyncWithExchangeAsync(UserId userId, CancellationToken cancellationToken);
}