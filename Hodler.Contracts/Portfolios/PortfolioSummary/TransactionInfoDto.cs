using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Contracts.Portfolios.PortfolioSummary;

public record TransactionInfoDto(
    Guid Id,
    TransactionType Type,
    FiatAmountDto FiatAmount,
    decimal BtcAmount,
    FiatAmountDto MarketPrice,
    DateTimeOffset Timestamp
);