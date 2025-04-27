using Corz.DomainDriven.Abstractions.DomainEvents;
using Corz.DomainDriven.Abstractions.Ports.Repositories;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Hodler.Integration.Repositories.BitcoinPrices.Repositories;

public class BitcoinPriceRepository : AggregateRootRepository<IBitcoinPrice>, IBitcoinPriceRepository
{
    // TODO: Implement. Store historic bitcoin prices in a database.

    public BitcoinPriceRepository(IDomainEventDispatcher domainEventDispatcher, ILogger<AggregateRootRepository<IBitcoinPrice>> logger) : base(
        domainEventDispatcher, logger)
    {
    }

    public Task<IReadOnlyCollection<IBitcoinPrice>> RetrievePricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    ) =>
        throw new NotImplementedException();

    public Task StoreAsync(IReadOnlyCollection<IBitcoinPrice> prices, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    protected override Task SaveChangesAsync(IBitcoinPrice aggregateRoot, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}