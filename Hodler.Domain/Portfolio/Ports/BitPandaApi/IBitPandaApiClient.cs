using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Ports.BitPandaApi;

public interface IBitPandaApiClient 
{
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
    
    Task<IFiatAmountCatalog> GetBitcoinPriceCatalogAsync(CancellationToken cancellationToken);
}