using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public PortfolioId PortfolioId { get; private set; }
    public UserId UserId { get; private set; }
    public ITransactions Transactions { get; private set; }

    public Portfolio(ITransactions transactions, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        Transactions = transactions;
        UserId = userId;
    }

    public IPortfolio SyncTransactions(IEnumerable<Transaction> transactions)
    {
        Transactions = Transactions.Sync(transactions);

        return this;
    }
}