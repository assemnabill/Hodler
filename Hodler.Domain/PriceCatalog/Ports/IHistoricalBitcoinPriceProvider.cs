using Corz.DomainDriven.Abstractions.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Ports;

public interface IHistoricalBitcoinPriceProvider
{
    // TODO: IMPLEMENT
    Task<Dictionary<DateOnly, FiatAmount>> GetBitcoinPriceOnDatesAsync(
        DateOnlyRange dateRange,
        FiatCurrency currency,
        CancellationToken cancellationToken = default
    );
}