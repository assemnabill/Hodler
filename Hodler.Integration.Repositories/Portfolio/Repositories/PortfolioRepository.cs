using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;

namespace Hodler.Integration.Repositories.Portfolio.Repositories;

internal class PortfolioRepository : IPortfolioRepository
{
    private Task SaveChangesAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        // TODO: Validation        

        return Task.CompletedTask;
    }

    public async Task StoreAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await this.SaveChangesAsync(aggregateRoot, cancellationToken);
        // TODO: Validation        

        // await this._domainEventDispatcher.PublishEventsOfAsync(aggregateRoot.DomainEventQueue, cancellationToken);
        aggregateRoot.OnAfterStore();
    }
}