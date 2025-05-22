using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public record TransactionInfo(
    TransactionId Id,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    FiatAmount MarketPrice,
    DateTimeOffset Timestamp,
    ITransactionSource? TransactionSource
);