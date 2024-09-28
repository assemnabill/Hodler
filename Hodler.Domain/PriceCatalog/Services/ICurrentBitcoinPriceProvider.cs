using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Services;

public interface ICurrentBitcoinPriceProvider
{
    Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken);
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken);
}