using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.BitcoinPrices.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Application.BitcoinPrices.Services;

public class HistoricalBitcoinPriceProvider : IHistoricalBitcoinPriceProvider
{
    private readonly IBitcoinPriceRepository _bitcoinPriceRepository;
    private readonly IBitcoinPriceSyncService _bitcoinPriceSyncService;

    public HistoricalBitcoinPriceProvider(
        IBitcoinPriceRepository bitcoinPriceRepository,
        IBitcoinPriceSyncService bitcoinPriceSyncService
    )
    {
        _bitcoinPriceRepository = bitcoinPriceRepository;
        _bitcoinPriceSyncService = bitcoinPriceSyncService;
    }

    public async Task<Dictionary<DateOnly, IBitcoinPrice>> GetHistoricalPriceOfDateIntervalAsync(
        FiatCurrency fiatCurrency,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {
        var resultExpectedCount = endDate.DayNumber - startDate.DayNumber + 1;

        var existingPrices = await _bitcoinPriceRepository
            .RetrievePricesAsync(fiatCurrency, startDate, endDate, cancellationToken);

        var result = existingPrices
            .ToDictionary(x => x.Date, x => x);

        if (result.Count == resultExpectedCount)
            return result;

        var missingDates = new List<DateOnly>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (result.ContainsKey(date))
                continue;

            missingDates.Add(date);
        }

        var missingPrices = await _bitcoinPriceSyncService.SyncMissingPricesAsync(
            fiatCurrency,
            missingDates.Min(),
            missingDates.Max(),
            cancellationToken
        );

        foreach (var price in missingPrices)
        {
            result.Add(price.Date, price);
        }

        return result;
    }

    public async Task<IBitcoinPrice> GetHistoricalPriceOnDateAsync(
        FiatCurrency fiatCurrency,
        DateOnly date,
        CancellationToken cancellationToken = default
    )
    {
        var bitcoinPrices = await GetHistoricalPriceOfDateIntervalAsync(fiatCurrency, date, date, cancellationToken);
        if (bitcoinPrices.TryGetValue(date, out var bitcoinPrice))
        {
            return bitcoinPrice;
        }

        throw new KeyNotFoundException($"The historical price for the date {date} could not be found.");
    }
}