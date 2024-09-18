using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    PortfolioId Id { get; }
    UserId UserId { get; }
    ITransactions Transactions { get; }

    SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions);
}