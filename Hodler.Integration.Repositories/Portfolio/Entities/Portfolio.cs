namespace Hodler.Integration.Repositories.Portfolio.Entities;

public class Portfolio : Entity
{
    public Guid PortfolioId { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }
}