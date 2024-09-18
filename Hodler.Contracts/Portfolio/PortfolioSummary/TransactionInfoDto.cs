namespace Hodler.Contracts.Portfolio.PortfolioSummary;

public record TransactionInfoDto(
    TransactionType Type,
    double FiatAmount,
    double BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp);