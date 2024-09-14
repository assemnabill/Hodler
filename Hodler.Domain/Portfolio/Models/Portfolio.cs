using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Models;

public class Portfolio : AggregateRoot<Portfolio>, IPortfolio
{
    public PortfolioId PortfolioId { get; private set; }
    public UserId UserId { get; private set; }
    public ITransactions Transactions { get; private set; }

    public Portfolio(PortfolioId portfolioId, ITransactions transactions, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        Transactions = transactions;
        UserId = userId;
        PortfolioId = portfolioId;
    }

    public SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        var syncResult = Transactions.Sync(transactions);

        if (syncResult.Changed)
        {
            Transactions = syncResult.CurrentState;
        }

        return new SyncResult<IPortfolio>(syncResult.Changed, this);
    }

    public static IPortfolio Create(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return new Portfolio(new PortfolioId(Guid.NewGuid()), new Transactions([]), userId);
    }
}