using Corz.Extensions.Enumeration;
using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Application.BitcoinPrices.Services;

public class HistoricalBitcoinPriceProvider : IHistoricalBitcoinPriceProvider
{
    private readonly IBitcoinPriceRepository _bitcoinPriceRepository;
    private readonly ICoinDeskApiClient _coinDeskApiClient;

    public HistoricalBitcoinPriceProvider(
        IBitcoinPriceRepository bitcoinPriceRepository,
        ICoinDeskApiClient coinDeskApiClient
    )
    {
        _bitcoinPriceRepository = bitcoinPriceRepository;
        _coinDeskApiClient = coinDeskApiClient;
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

        var missingPrices = await SyncMissingPricesAsync(
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