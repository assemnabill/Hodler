using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Services;

public interface IPriceCatalogService
{
    // TODO: Implement Command. For SignalR use
    Task<IPriceCatalog<CryptoCurrency>> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken = default);
}