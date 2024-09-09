using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Portfolio.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    ITransactions Transactions { get; }
    
    IPortfolio SyncTransactions(IEnumerable<Transaction> transactions);
}