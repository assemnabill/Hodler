namespace Hodler.Integration.Repositories.Portfolio.Entities;

public class Transaction : Entity
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public int Type { get; set; }
    public double FiatAmount { get; set; }
    public double BtcAmount { get; set; }
    public double MarketPrice { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int CryptoExchange { get; set; }
    public int FiatCurrency { get; set; }

    public virtual Portfolio Portfolio { get; set; }
}