namespace Hodler.Integration.Repositories.Portfolios.Entities;

public class Portfolio : Entity
{
    public Guid PortfolioId { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<ManualTransaction> ManualTransactions { get; set; }
    public virtual ICollection<BitcoinWallet> BitcoinWallets { get; set; }
}