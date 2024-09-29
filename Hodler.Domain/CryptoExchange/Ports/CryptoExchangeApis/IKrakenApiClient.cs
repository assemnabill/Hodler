using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.CryptoExchange.Ports.CryptoExchangeApis;

public interface IKrakenApiClient
{
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
}