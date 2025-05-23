namespace Hodler.Integration.Repositories.Portfolios.Entities;

public class Transaction : Entity
{
    public Guid TransactionId { get; set; }
    public Guid PortfolioId { get; init; }
    public int Type { get; init; }
    public decimal FiatAmount { get; init; }
    public decimal BtcAmount { get; init; }
    public decimal MarketPrice { get; init; }
    public decimal Fee { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public int FiatCurrency { get; init; }
    public int? SourceType { get; init; }
    public string? SourceIdentifier { get; init; }
    public string? SourceName { get; init; }


    public virtual Portfolio Portfolio { get; init; }
}