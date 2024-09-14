namespace Hodler.Integration.Repositories.Portfolio.Entities;

public class Transaction : Entity
{
    public Guid TransactionId { get; init; }
    public Guid PortfolioId { get; init; }
    public int Type { get; init; }
    public double FiatAmount { get; init; }
    public double BtcAmount { get; init; }
    public double MarketPrice { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public int CryptoExchange { get; init; }
    public int FiatCurrency { get; init; }

    public virtual Portfolio Portfolio { get; init; }
}