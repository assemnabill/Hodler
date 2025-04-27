using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface IHistoricalBitcoinPriceProvider
{
    // TODO: Implement using coin desk api
    Task<IBitcoinPrice> GetHistoricalPriceOnDateAsync(
        FiatCurrency fiatCurrency,
        DateOnly date,
        CancellationToken cancellationToken = default
    );

    // TODO: Implement using coin desk api
    Task<Dictionary<DateOnly, IBitcoinPrice>> GetHistoricalPriceOfDateIntervalAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
}