using System.Globalization;
using Corz.DomainDriven.Abstractions.Models;
using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBitcoinPrice;

public class CoinCapHistoricalBitcoinPriceProvider : IHistoricalBitcoinPriceProvider
{
    private readonly IBitcoinPriceRepository _bitcoinPriceRepository;
    private readonly IDistributedCache _cache;
    private readonly ICoinCapApiClient _coinCapApiClient;

    public CoinCapHistoricalBitcoinPriceProvider(
        ICoinCapApiClient coinCapApiClient,
        IDistributedCache cache,
        IBitcoinPriceRepository bitcoinPriceRepository
    )
    {
        _coinCapApiClient = coinCapApiClient;
        _cache = cache;
        _bitcoinPriceRepository = bitcoinPriceRepository;
    }

    public async Task<Dictionary<DateOnly, FiatAmount>> GetBitcoinPriceOnDatesAsync(
        DateOnlyRange dateRange,
        FiatCurrency currency,
        CancellationToken cancellationToken = default
    )
    {
        var dates = new List<DateOnly>();
        for (DateOnly i = dateRange.Start; i <= dateRange.End; i = i.AddDays(1))
        {
            dates.Add(i);
        }

        var cachedDates = await GetCachedDatesAsync(dates, cancellationToken, out var missingDates);

        if (missingDates.Count != 0)
        {
            var missingPrices = await FetchMissingPricesAsync(missingDates, cancellationToken);
            missingDates.ForEach(date => cachedDates.Add(date, missingPrices[date]));
            CachePricesAsync(missingPrices, cancellationToken);
        }

        var priceOnDates = cachedDates
            .ToDictionary(
                x => x.Key,
                x => x.Value.GetPrice(currency)
            );

        return priceOnDates;
    }

    private async Task CachePricesAsync(
        Dictionary<DateOnly, IFiatAmountCatalog> missingPrices,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<DateOnly, IFiatAmountCatalog>> FetchMissingPricesAsync(
        List<DateOnly> missingDates,
        CancellationToken cancellationToken
    )
    {
        var startDate = missingDates.Min();
        var endDate = missingDates.Max();

        // try to fetch from db

        // if not in db, fetch from api

        // store in db asynchronously


        // return response
        throw new NotImplementedException();
    }

    private Task<Dictionary<DateOnly, IFiatAmountCatalog>> GetCachedDatesAsync(
        List<DateOnly> dates,
        CancellationToken cancellationToken,
        out List<DateOnly> missingDays
    )
    {
        throw new NotImplementedException();
    }

    private static string CacheKey(DateOnly date) => date.ToString(CultureInfo.InvariantCulture);
}