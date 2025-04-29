using System.Diagnostics;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.BitcoinPrices.Repositories;

public class BitcoinPriceRepository : IBitcoinPriceRepository
{
    private readonly BitcoinPriceDbContext _dbContext;
    private readonly ILogger<BitcoinPriceRepository> _logger;

    public BitcoinPriceRepository(
        BitcoinPriceDbContext dbContext,
        ILogger<BitcoinPriceRepository> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;

    }

    public async Task<IReadOnlyCollection<IBitcoinPrice>> RetrievePricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(fiatCurrency);

        var prices = await _dbContext.BitcoinPrices
            .Where(p => p.Currency == fiatCurrency.Id
                        && p.Date >= startDate
                        && p.Date <= endDate)
            .Select(p => p.Adapt<BitcoinPrice>())
            .ToListAsync(cancellationToken);

        return prices;
    }

    public async Task StoreAsync(IReadOnlyCollection<IBitcoinPrice> prices, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prices);

        if (!prices.Any())
            return;

        cancellationToken.ThrowIfCancellationRequested();

        if (_dbContext.Database.CurrentTransaction is null)
            _logger.LogWarning(
                "No active database transaction found while storing bitcoin prices. Stack Trace: {StackTrace}.",
                new StackTrace()
            );

        try
        {
            foreach (var price in prices)
            {
                price.OnBeforeStore();
            }

            var existingPrices = await _dbContext.BitcoinPrices
                .Where(p => prices.Any(x => x.Date == p.Date && x.Currency.Id == p.Currency))
                .ToDictionaryAsync(
                    p => (p.Date, p.Currency),
                    p => p,
                    cancellationToken
                );

            foreach (var price in prices)
            {
                var entity = price.Adapt<Entities.BitcoinPrice>();

                if (existingPrices.TryGetValue((price.Date, price.Currency.Id), out var existingEntity))
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    await _dbContext.BitcoinPrices.AddAsync(entity, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var price in prices)
            {
                price.OnAfterStore();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while storing bitcoin prices");
            throw;
        }
    }


    public async Task StoreAsync(IBitcoinPrice aggregateRoot, CancellationToken cancellationToken)
    {
        aggregateRoot.OnBeforeStore();
        await SaveChangesAsync(aggregateRoot, cancellationToken);
        aggregateRoot.OnAfterStore();
    }

    private async Task SaveChangesAsync(IBitcoinPrice aggregateRoot, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregateRoot);

        cancellationToken.ThrowIfCancellationRequested();

        if (_dbContext.Database.CurrentTransaction is null)
            _logger.LogWarning
            (
                "No active database transaction found while storing portfolio ({AggregateRootDate}). Stack Trace: {StackTrace}.", aggregateRoot.Date,
                new StackTrace()
            );

        try
        {
            var existingDbEntity = await _dbContext.BitcoinPrices
                .FirstOrDefaultAsync(t => t.Date == aggregateRoot.Date
                                          && t.Currency == aggregateRoot.Currency.Id,
                    cancellationToken
                );

            var entity = aggregateRoot.Adapt<Entities.BitcoinPrice>();
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

            var rows = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Error while storing portfolio ({AggregateRootDate}). Stack Trace: {StackTrace}.", aggregateRoot.Date, new StackTrace());
        }
    }
}