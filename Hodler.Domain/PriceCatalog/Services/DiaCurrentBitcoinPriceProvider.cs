using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Shared.Ports.DiaDataApi;

namespace Hodler.Domain.PriceCatalog.Services;

public class DiaCurrentBitcoinPriceProvider : ICurrentBitcoinPriceProvider
{
    private readonly IDiaDataApiClient _diaDataApiClient;

    public DiaCurrentBitcoinPriceProvider(IDiaDataApiClient diaDataApiClient)
    {
        _diaDataApiClient = diaDataApiClient;
    }

    public async Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken)
    {
        var qoutation = await _diaDataApiClient.GetBitcoinAssetQuotationAsync(cancellationToken);

        return new FiatAmount(qoutation!.Price, FiatCurrency.UsDollar);
    }

    public async Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken)
    {
        return new FiatAmountCatalog([
            await GetCurrentBitcoinPriceInAmericanDollarsAsync(cancellationToken)
        ]);
    }
}