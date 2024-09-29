using System.Diagnostics;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.User.Models;
using Hodler.Integration.Repositories.Portfolio.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
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
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(t => t.PortfolioId == aggregateRoot.Id, cancellationToken);

            var entity = aggregateRoot.Adapt<IPortfolio, Portfolio.Entities.Portfolio>();
            if (existingDbEntity is null)
            {
                entity.CreatedAt = DateTimeOffset.UtcNow;
                await _dbContext.AddAsync(entity, cancellationToken);
            }
            else
            {
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                _dbContext.Entry(existingDbEntity).CurrentValues.SetValues(entity);
                _dbContext.Entry(existingDbEntity).State = EntityState.Modified;
            }

            await ChangeTransactions(aggregateRoot, entity, cancellationToken);

            int rows = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while storing portfolio ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}.");
        }
    }

    private async Task ChangeTransactions(
        IPortfolio aggregateRoot,
        Entities.Portfolio entity,
        CancellationToken cancellationToken)
    {
        var existingEntities = _dbContext.Transactions
            .Where(x => x.PortfolioId == entity.PortfolioId)
            .ToList();

        var newEntities = aggregateRoot.Transactions
            .Select(x => x.Adapt<Transaction, Portfolio.Entities.Transaction>());

        foreach (var newEntity in newEntities)
        {
            var existingEntity = existingEntities
                .FirstOrDefault(x => x.Timestamp == newEntity.Timestamp
                                     && x.Type == newEntity.Type
                                     && x.FiatCurrency == newEntity.FiatCurrency
                                     && x.PortfolioId == newEntity.PortfolioId
                                     && x.CryptoExchange == newEntity.CryptoExchange
                                     && x.FiatAmount == newEntity.FiatAmount
                                     && x.BtcAmount == newEntity.BtcAmount
                                     && x.MarketPrice == newEntity.MarketPrice
                );

            if (existingEntity is null)
            {
                newEntity.CreatedAt = DateTimeOffset.UtcNow;
                await _dbContext.AddAsync(newEntity, cancellationToken);
            }
        }
    }

    public async Task StoreAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
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

    private IQueryable<Entities.Portfolio> IncludeAggregate()
    {
        return _dbContext.Portfolios
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Transactions);
    }
}