using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.User.Models;
using Hodler.Integration.Repositories.Portfolio.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hodler.Integration.Repositories.Portfolio.Repositories;

internal class PortfolioRepository : IPortfolioRepository
{
    private readonly PortfolioDbContext _dbContext;

    public PortfolioRepository(PortfolioDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task SaveChangesAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        // TODO: Validation        
        var entity = aggregateRoot.Adapt<IPortfolio, Portfolio.Entities.Portfolio>();

        _dbContext.Portfolios.Add(entity);

        ChangeTransactions(aggregateRoot, entity);

        int rows = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ChangeTransactions(IPortfolio aggregateRoot, Portfolio.Entities.Portfolio entity)
    {
        var transactions = aggregateRoot.Transactions
            .Select(x => x.Adapt<Transaction, Portfolio.Entities.Transaction>());

        _dbContext.Transactions.AddRange(transactions);
    }

    public async Task StoreAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
        // TODO: Validation        
        // await this._domainEventDispatcher.PublishEventsOfAsync(aggregateRoot.DomainEventQueue, cancellationToken);
        aggregateRoot.OnAfterStore();
    }

    public async Task<IPortfolio?> FindByAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var entity = await IncludeAggregate()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.Adapt<Portfolio.Entities.Portfolio, IPortfolio>();
    }

    private IIncludableQueryable<Entities.Portfolio, ICollection<Entities.Transaction>> IncludeAggregate()
    {
        return _dbContext.Portfolios
            .AsSingleQuery()
            .Include(x => x.Transactions);
    }
}