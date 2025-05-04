using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public record TransactionInfo(
    TransactionId Id,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    FiatAmount MarketPrice,
    DateTimeOffset Timestamp,
    CryptoExchangeName? CryptoExchange
);