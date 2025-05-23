using System.Diagnostics;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Users.Models;
using Hodler.Integration.Repositories.Portfolios.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BitcoinWallet = Hodler.Integration.Repositories.Portfolios.Entities.BitcoinWallet;
using Portfolio = Hodler.Integration.Repositories.Portfolios.Entities.Portfolio;

namespace Hodler.Integration.Repositories.Portfolios.Repositories;

internal class PortfolioRepository : IPortfolioRepository
{
    private readonly PortfolioDbContext _context;
    private readonly ILogger<PortfolioRepository> _logger;

    public PortfolioRepository(
        PortfolioDbContext context,
        ILogger<PortfolioRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task StoreAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
        aggregateRoot.OnAfterStore();
    }

    public async Task<IPortfolio?> FindByAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var entity = await IncludeAggregate()
            .Where(x => x.UserId == userId.Value.ToString())
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.Adapt<Portfolio, IPortfolio>();
    }

    public async Task<IPortfolio?> FindByAsync(
        PortfolioId portfolioId,
        CancellationToken cancellationToken = default
    )
    {
        var portfolio = await IncludeAggregate()
            .FirstOrDefaultAsync(x => x.PortfolioId == portfolioId.Value, cancellationToken);

        return portfolio?.Adapt<IPortfolio>();
    }


    public async Task<IPortfolio?> FindByAsync(
        BitcoinWalletId walletId,
        CancellationToken cancellationToken = default
    )
    {
        var portfolio = await IncludeAggregate()
            .FirstOrDefaultAsync(x => x.BitcoinWallets.Any(w => w.BitcoinWalletId == walletId.Value), cancellationToken);

        return portfolio?.Adapt<IPortfolio>();
    }

    private async Task SaveChangesAsync(IPortfolio aggregateRoot, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregateRoot);

        cancellationToken.ThrowIfCancellationRequested();

        if (_context.Database.CurrentTransaction is null)
            _logger.LogWarning
            (
                $"No active database transaction found while storing portfolio ({aggregateRoot.Id}). Stack Trace: {new StackTrace()}."
            );

        try
        {
            var existingDbEntity = await _context.Portfolios
                .Include(x => x.Transactions)
                .Include(x => x.BitcoinWallets)
                .FirstOrDefaultAsync(t => t.PortfolioId == aggregateRoot.Id, cancellationToken);

            var entity = aggregateRoot.Adapt<IPortfolio, Portfolio>();
            if (existingDbEntity is null)
            {
                entity.CreatedAt = DateTimeOffset.UtcNow;
                await _context.AddAsync(entity, cancellationToken);
            }
            else
            {
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                _context.Entry(existingDbEntity).CurrentValues.SetValues(entity);
                _context.Entry(existingDbEntity).State = EntityState.Modified;
            }

            await ChangeTransactions(aggregateRoot, entity, cancellationToken);
            await ChangeBitcoinWallets(aggregateRoot, entity, cancellationToken);

            var rows = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while storing portfolio ({aggregateRoot.Id}). Stack Trace: {new StackTrace(e)}.");
            throw;
        }
    }

    private async Task ChangeBitcoinWallets(IPortfolio aggregateRoot, Portfolio entity, CancellationToken cancellationToken)
    {
        var existingEntities = _context.BitcoinWallets
            .Where(x => x.PortfolioId == entity.PortfolioId)
            .ToList();

        var newEntities = aggregateRoot.BitcoinWallets
            .Select(x => x.Adapt<IBitcoinWallet, BitcoinWallet>())
            .ToList();

        var toRemove = existingEntities.Except(newEntities).ToList();
        _context.RemoveRange(toRemove);

        foreach (var newEntity in newEntities)
        {
            var existingEntity = existingEntities
                .FirstOrDefault(x => x.BitcoinWalletId == newEntity.BitcoinWalletId);

            if (existingEntity is not null)
            {
                newEntity.UpdatedAt = DateTimeOffset.UtcNow;
                _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                _context.Entry(existingEntity).State = EntityState.Modified;
                continue;
            }

            newEntity.CreatedAt = DateTimeOffset.UtcNow;
            await _context.AddAsync(newEntity, cancellationToken);
        }
    }

    private async Task ChangeTransactions(
        IPortfolio aggregateRoot,
        Portfolio entity,
        CancellationToken cancellationToken
    )
    {
        var existingEntities = _context.Transactions
            .Where(x => x.PortfolioId == entity.PortfolioId)
            .ToList();

        var newEntities = aggregateRoot.Transactions
            .Select(x => x.Adapt<Transaction, Entities.Transaction>())
            .ToList();

        var toRemove = existingEntities.Except(newEntities).ToList();
        _context.RemoveRange(toRemove);

        foreach (var newEntity in newEntities)
        {
            var existingEntity = existingEntities
                .FirstOrDefault(x => x.TransactionId == newEntity.TransactionId);

            if (existingEntity is not null)
            {
                newEntity.UpdatedAt = DateTimeOffset.UtcNow;
                _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                _context.Entry(existingEntity).State = EntityState.Modified;
                continue;
            }

            newEntity.CreatedAt = DateTimeOffset.UtcNow;
            await _context.AddAsync(newEntity, cancellationToken);
        }
    }

    private IQueryable<Portfolio> IncludeAggregate()
    {
        return _context.Portfolios
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Transactions)
            .Include(x => x.BitcoinWallets);
    }
}