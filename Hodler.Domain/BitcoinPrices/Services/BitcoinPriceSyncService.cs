using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Services;

public class BitcoinPriceSyncService(
    ICoinDeskApiClient coinDeskApiClient,
    IBitcoinPriceRepository bitcoinPriceRepository
) : IBitcoinPriceSyncService
{
    public async Task<IReadOnlyCollection<IBitcoinPrice>> SyncMissingPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {
        var prices = await coinDeskApiClient
            .GetHistoricalDailyBitcoinPricesAsync(fiatCurrency, startDate, endDate, cancellationToken);

        if (!prices.IsNullOrEmpty())
            await bitcoinPriceRepository.StoreAsync(prices, cancellationToken);

        return prices;
    }
}