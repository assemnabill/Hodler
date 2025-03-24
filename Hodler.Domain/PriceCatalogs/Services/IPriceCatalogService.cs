using Hodler.Domain.PriceCatalogs.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalogs.Services;

public interface IPriceCatalogService
{
    // TODO: Implement Command. For SignalR use
    Task<IPriceCatalog<CryptoCurrency>> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}