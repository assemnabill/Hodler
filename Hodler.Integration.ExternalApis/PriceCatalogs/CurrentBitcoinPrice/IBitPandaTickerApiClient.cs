using Hodler.Domain.BitcoinPrices.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.CurrentBitcoinPrice;

public interface IBitPandaTickerApiClient
{
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(
        CancellationToken cancellationToken = default
    );
}