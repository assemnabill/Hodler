using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Ports.BitPandaApi;

public interface IBitPandaApiClient 
{
    Task<ITransactions> GetTransactionsAsync(UserId userId, CancellationToken cancellationToken);
}