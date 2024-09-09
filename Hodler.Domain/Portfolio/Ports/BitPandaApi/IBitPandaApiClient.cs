using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Ports.BitPandaApi;

public interface IBitPandaApiClient 
{
    Task<ITransactions> GetTransactionsAsync(Guid userId, CancellationToken cancellationToken);
}