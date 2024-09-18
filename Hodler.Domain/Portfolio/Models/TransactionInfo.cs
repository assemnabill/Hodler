using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public record TransactionInfo(
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp);