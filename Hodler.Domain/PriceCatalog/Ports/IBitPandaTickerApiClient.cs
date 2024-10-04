using Hodler.Domain.PriceCatalog.Models;

namespace Hodler.Domain.PriceCatalog.Ports;

public interface IBitPandaTickerApiClient 
{
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken);
}