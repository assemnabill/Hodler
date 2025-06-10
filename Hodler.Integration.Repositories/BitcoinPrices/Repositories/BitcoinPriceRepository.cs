using System.Diagnostics;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;
using Hodler.Integration.Repositories.BitcoinPrices.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BitcoinPrice = Hodler.Integration.Repositories.BitcoinPrices.Entities.BitcoinPrice;

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
            .Select(p => p.Adapt<IBitcoinPrice>())
            .ToListAsync(cancellationToken);

        return prices;
    }

    public async Task StoreAsync(IReadOnlyCollection<IBitcoinPrice> prices, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prices);

        if (prices.Count == 0)
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
                price.OnBeforeStore();

            var toBeInserted = new List<BitcoinPrice>();

            foreach (var price in prices)
            {
                var existingEntity = await _dbContext.BitcoinPrices
                    .FirstOrDefaultAsync(
                        x => x.Date == price.Date && x.Currency == price.Currency.Id,
                        cancellationToken: cancellationToken
                    );

                if (existingEntity is not null)
                    continue;

                var entity = price.Adapt<BitcoinPrice>();
                entity.CreatedAt = DateTimeOffset.UtcNow;
                toBeInserted.Add(entity);
            }

            if (toBeInserted.Count != 0)
                await _dbContext.BitcoinPrices.AddRangeAsync(toBeInserted, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var price in prices)
                price.OnAfterStore();

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while storing bitcoin prices");
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

            var entity = aggregateRoot.Adapt<BitcoinPrice>();
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
                "Error while storing portfolio ({AggregateRootDate}). Stack Trace: {StackTrace}.",
                aggregateRoot.Date,
                new StackTrace()
            );
        }
    }
}