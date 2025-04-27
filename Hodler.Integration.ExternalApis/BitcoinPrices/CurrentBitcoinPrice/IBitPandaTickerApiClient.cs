using Hodler.Domain.BitcoinPrices.Models;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.CurrentBitcoinPrice;

public interface IBitPandaTickerApiClient
{
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(
        CancellationToken cancellationToken = default
    );
}