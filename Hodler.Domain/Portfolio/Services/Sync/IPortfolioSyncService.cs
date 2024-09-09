using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Services.Sync;

public interface IPortfolioSyncService
{
    Task<IPortfolio> SyncWithExchangeAsync(Guid userId, CancellationToken cancellationToken);
}