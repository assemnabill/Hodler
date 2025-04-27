using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface ICoinDeskApiClient
{
    Task<IReadOnlyCollection<IBitcoinPrice>> GetHistoricalDailyBitcoinPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
}