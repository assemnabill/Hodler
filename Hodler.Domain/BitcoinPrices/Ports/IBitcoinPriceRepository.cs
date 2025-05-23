using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface IBitcoinPriceRepository : IRepository<IBitcoinPrice>
{
    Task<IReadOnlyCollection<IBitcoinPrice>> RetrievePricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );

    Task StoreAsync(
        IReadOnlyCollection<IBitcoinPrice> prices,
        CancellationToken cancellationToken = default
    );
}