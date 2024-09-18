using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface ITransactionsQueryService
{
    Task<PortfolioSummary> GetPortfolioSummaryAsync(UserId userId, CancellationToken cancellationToken);
    Task<ITransactions> GetTransactionsAsync(CancellationToken cancellationToken);
}