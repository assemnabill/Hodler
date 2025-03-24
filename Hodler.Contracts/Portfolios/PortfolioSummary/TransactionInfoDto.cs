namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record TransactionInfoDto(
    TransactionType Type,
    decimal FiatAmount,
    decimal BtcAmount,
    decimal MarketPrice,
    DateTimeOffset Timestamp);