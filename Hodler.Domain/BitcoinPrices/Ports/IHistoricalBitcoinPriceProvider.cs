using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface IHistoricalBitcoinPriceProvider
{
    // TODO: Implement using coin desk api
    Task<FiatAmount> GetHistoricalPriceOnDateAsync(
        FiatCurrency fiatCurrency,
        DateOnly date,
        CancellationToken cancellationToken = default
    );

    // TODO: Implement using coin desk api
    Task<Dictionary<DateOnly, FiatAmount>> GetHistoricalPriceOfDateIntervalAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
}