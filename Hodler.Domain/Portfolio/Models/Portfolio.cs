using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Portfolio.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public ITransactions Transactions { get; private set; }

    public Portfolio(ITransactions transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        Transactions = transactions;
    }
    
    public IPortfolio SyncTransactions(IEnumerable<Transaction> transactions)
    {
        
        Transactions = Transactions.Sync(transactions);
        
        return this;
    }
}