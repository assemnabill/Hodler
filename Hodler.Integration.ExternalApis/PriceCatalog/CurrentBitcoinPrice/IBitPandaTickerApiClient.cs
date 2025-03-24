using Hodler.Domain.PriceCatalog.Models;

namespace Hodler.Integration.ExternalApis.PriceCatalog.CurrentBitcoinPrice;

public interface IBitPandaTickerApiClient
{
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(
        CancellationToken cancellationToken = default
    );
}