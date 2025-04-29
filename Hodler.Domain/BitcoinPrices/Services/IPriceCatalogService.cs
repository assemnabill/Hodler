using Hodler.Domain.BitcoinPrices.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Services;

public interface IPriceCatalogService
{
    // TODO: Implement Command. For SignalR use
    Task<IPriceCatalog<CryptoCurrency>> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}