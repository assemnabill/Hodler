using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public record TransactionInfo(
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    FiatAmount MarketPrice,
    DateTimeOffset Timestamp,
    CryptoExchangeNames? CryptoExchange);