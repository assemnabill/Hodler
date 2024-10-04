using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.PriceCatalog.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Services;

public class BitPandaCurrentBitcoinPriceProvider : ICurrentBitcoinPriceProvider
{
    private readonly IBitPandaTickerApiClient _bitPandaTickerApiClient;

    public BitPandaCurrentBitcoinPriceProvider(IBitPandaTickerApiClient bitPandaTickerApiClient)
    {
        _bitPandaTickerApiClient = bitPandaTickerApiClient;
    }

    public async Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken)
    {
        var priceCatalog = await GetBitcoinPriceCatalogAsync(cancellationToken);
        
        return priceCatalog.GetPrice(FiatCurrency.UsDollar);
    }

    public async Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken)
    {
        return await _bitPandaTickerApiClient.GetBitcoinPriceCatalogAsync(cancellationToken);
    }
}