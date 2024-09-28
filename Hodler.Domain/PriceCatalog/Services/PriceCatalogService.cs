using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Domain.PriceCatalog.Services;

public class PriceCatalogService : IPriceCatalogService
{
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;

    public PriceCatalogService(
        [FromKeyedServices(BitcoinPriceProvider.BitPandaTicker)]
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