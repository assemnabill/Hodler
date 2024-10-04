using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.CryptoExchange.Ports.CryptoExchangeApis;

public interface IBitPandaSpotApiClient 
{
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
}