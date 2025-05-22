using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;

public interface IKrakenApiClient
{
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
}