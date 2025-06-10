namespace Hodler.Integration.Repositories.Portfolios.Entities;

public class ManualTransaction : Entity
{
    public Guid TransactionId { get; set; }
    public Guid PortfolioId { get; set; }
    public int Type { get; set; }
    public decimal BtcAmount { get; set; }
    public decimal FiatAmount { get; set; }
    public decimal MarketPrice { get; set; }
    public decimal? Fee { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int FiatCurrency { get; set; }
    public int? SourceType { get; set; }
    public string? SourceIdentifier { get; set; }
    public string? SourceName { get; set; }


    public virtual Portfolio Portfolio { get; init; }
}