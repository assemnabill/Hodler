using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Services;

public class BitcoinPriceSyncService : IBitcoinPriceSyncService
{
    private readonly ICoinDeskApiClient _coinDeskApiClient;
    private readonly IBitcoinPriceRepository _bitcoinPriceRepository;

    public BitcoinPriceSyncService(
        ICoinDeskApiClient coinDeskApiClient,
        IBitcoinPriceRepository bitcoinPriceRepository
    )
    {
        _coinDeskApiClient = coinDeskApiClient;
        _bitcoinPriceRepository = bitcoinPriceRepository;
    }

    public async Task<IReadOnlyCollection<IBitcoinPrice>> SyncMissingPricesAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {

        var prices = await _coinDeskApiClient
            .GetHistoricalDailyBitcoinPricesAsync(fiatCurrency, startDate, endDate, cancellationToken);

        if (!prices.IsNullOrEmpty())
        {
            await _bitcoinPriceRepository.StoreAsync(prices, cancellationToken);
        }

        return prices;
    }
}