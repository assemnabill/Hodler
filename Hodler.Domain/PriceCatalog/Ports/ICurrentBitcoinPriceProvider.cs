using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Ports;

public interface ICurrentBitcoinPriceProvider
{
    Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken = default);
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}