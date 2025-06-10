namespace Hodler.Integration.Repositories.Portfolios.Entities;

public class BlockchainTransaction : Entity
{
    public Guid PortfolioId { get; set; }
    public Guid BitcoinWalletId { get; set; }
    public decimal BtcAmount { get; set; }
    public string TransactionHash { get; set; }
    public decimal MarketPriceInUsd { get; set; }
    public decimal FiatValueInUsd { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int Status { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public decimal NetworkFeeInBtc { get; set; }
    public decimal NetworkFeeInUsd { get; set; }
    public string? Note { get; set; }
    public int Type { get; set; }


    public virtual Portfolio Portfolio { get; init; }
    public virtual BitcoinWallet BitcoinWallet { get; init; }
}