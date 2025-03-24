using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;

public interface IBitPandaSpotApiClient 
{
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
}