using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public record Transaction(
    PortfolioId PortfolioId,
    TransactionId Id,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    DateTimeOffset Timestamp,
    CryptoExchangeName? CryptoExchange = null
)
{
    public FiatAmount MarketPrice =>
        BtcAmount == 0
            ? new(0, FiatAmount.FiatCurrency)
            : new(FiatAmount / BtcAmount, FiatAmount.FiatCurrency);

    public bool IsInCurrency(FiatCurrency fiatCurrency) => FiatAmount.FiatCurrency.Id == fiatCurrency.Id;
}