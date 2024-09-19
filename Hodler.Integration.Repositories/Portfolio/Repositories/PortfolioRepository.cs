using System.Diagnostics;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.User.Models;
using Hodler.Integration.Repositories.Portfolio.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.Portfolio.Repositories;

internal class PortfolioRepository : IPortfolioRepository
{
    private readonly PortfolioDbContext _dbContext;
    private readonly ILogger<PortfolioRepository> _logger;

    public PortfolioRepository(
        PortfolioDbContext dbContext,
        ILogger<PortfolioRepository> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    private async Task SaveChangesAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregateRoot);

        cancellationToken.ThrowIfCancellationRequested();

        if (_dbContext.Database.CurrentTransaction is null)
            _logger.LogWarning
            (
                $"No active database transaction found while storing portfolio ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}."
            );

        try
        {
            var existingDbEntity = await _dbContext.Portfolios
                .AsNoTracking()
                .AsSingleQuery()
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(t => t.PortfolioId == aggregateRoot.Id, cancellationToken);

            var entity = aggregateRoot.Adapt<IPortfolio, Portfolio.Entities.Portfolio>();
            if (existingDbEntity is null)
            {
                await _dbContext.AddAsync(entity, cancellationToken);
            }
            else
            {
                _dbContext.Entry(existingDbEntity).CurrentValues.SetValues(entity);
                _dbContext.Entry(existingDbEntity).State = EntityState.Modified;
            }
            // _dbContext.Portfolios.Update(entity);
            // todo
            // ChangeTransactions(aggregateRoot, entity);

            int rows = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while storing portfolio ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}.");
        }
    }

    private void ChangeTransactions(IPortfolio aggregateRoot, Portfolio.Entities.Portfolio entity)
    {
        var existingTransactions = entity.Transactions;
        var transactions = aggregateRoot.Transactions
            .Select(x => x.Adapt<Transaction, Portfolio.Entities.Transaction>());

        foreach (var transaction in transactions)
        {
            var existingTransaction = existingTransactions.FirstOrDefault(x => x.Equals(transaction));

            if (existingTransaction is null)
                _dbContext.Transactions.Add(transaction);
        }

        // _dbContext.Transactions.AddRange(transactions);
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
            .Where(x => x.UserId == userId.Value.ToString())
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.Adapt<Portfolio.Entities.Portfolio, IPortfolio>();
    }

    private IIncludableQueryable<Entities.Portfolio, ICollection<Entities.Transaction>> IncludeAggregate()
    {
        return _dbContext.Portfolios
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Transactions);
    }
}