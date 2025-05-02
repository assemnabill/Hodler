using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Services;

public interface IBitcoinPriceSyncService
{
    public Task<IReadOnlyCollection<IBitcoinPrice>> SyncMissingPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
}