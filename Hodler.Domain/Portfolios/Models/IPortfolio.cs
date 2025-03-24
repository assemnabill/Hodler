using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    PortfolioId Id { get; }
    UserId UserId { get; }
    ITransactions Transactions { get; }

    SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions);
}