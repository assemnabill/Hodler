using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Ports;

public interface ICurrentBitcoinPriceProvider
{
    Task<FiatAmount> GetCurrentBitcoinPriceInAmericanDollarsAsync(CancellationToken cancellationToken = default);
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}