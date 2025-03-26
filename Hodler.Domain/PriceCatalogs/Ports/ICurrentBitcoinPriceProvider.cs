using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalogs.Ports;

public interface ICurrentBitcoinPriceProvider
{
    Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken = default);
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}