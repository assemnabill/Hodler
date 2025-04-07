using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record TransactionInfoDto(
    TransactionType Type,
    FiatAmountDto FiatAmount,
    decimal BtcAmount,
    FiatAmountDto MarketPrice,
    DateTimeOffset Timestamp
);