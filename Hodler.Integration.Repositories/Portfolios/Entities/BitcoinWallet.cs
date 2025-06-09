namespace Hodler.Integration.Repositories.Portfolios.Entities;

public class BitcoinWallet : Entity
{
    public Guid BitcoinWalletId { get; set; }
    public Guid PortfolioId { get; set; }
    public string Address { get; set; }
    public string WalletName { get; set; }
    public int Network { get; set; }
    public DateTimeOffset ConnectedDate { get; set; }
    public DateTimeOffset? LastSynced { get; set; }
    public decimal Balance { get; set; }

    public virtual Portfolio Portfolio { get; set; }
    public virtual ICollection<BlockchainTransaction> BlockchainTransactions { get; set; }
}