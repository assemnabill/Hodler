using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalogs.Services;

public class PriceCatalogService : IPriceCatalogService
{
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;

    public PriceCatalogService(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider)
    {
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
    }

    public async Task<IPriceCatalog<CryptoCurrency>> GetBitcoinPriceCatalogAsync(
        CancellationToken cancellationToken = default)
    {
        var bitcoinPrice = await _currentBitcoinPriceProvider.GetBitcoinPriceCatalogAsync(cancellationToken);

        var bitcoinPriceCatalog = new CryptoCurrencyPriceCatalog
        {
            { CryptoCurrency.Bitcoin, bitcoinPrice }
        };

        return bitcoinPriceCatalog;
    }
}