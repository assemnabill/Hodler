using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface IHistoricalBitcoinPriceProvider
{
    Task<Dictionary<DateOnly, IBitcoinPrice>> GetHistoricalPriceOfDateIntervalAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );

    public Task<IReadOnlyCollection<IBitcoinPrice>> SyncMissingPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
}