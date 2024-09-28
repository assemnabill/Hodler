using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Services;

public class BitPandaCurrentBitcoinPriceProvider : ICurrentBitcoinPriceProvider
{
    private readonly IBitPandaApiClient _bitPandaApiClient;

    public BitPandaCurrentBitcoinPriceProvider(IBitPandaApiClient bitPandaApiClient)
    {
        _bitPandaApiClient = bitPandaApiClient;
    }

    public async Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken)
    {
        var priceCatalog = await GetBitcoinPriceCatalogAsync(cancellationToken);
        
        return priceCatalog.GetPrice(FiatCurrency.UsDollar);
    }

    public async Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken)
    {
        return await _bitPandaApiClient.GetBitcoinPriceCatalogAsync(cancellationToken);
    }
}