using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public record Transaction(
    PortfolioId PortfolioId,
    TransactionId Id,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    DateTimeOffset Timestamp,
    FiatAmount MarketPrice,
    ITransactionSource? TransactionSource,
    BitcoinAmount? TransactionFee = null
)
{
    public bool IsInCurrency(FiatCurrency fiatCurrency) => FiatAmount.FiatCurrency.Id == fiatCurrency.Id;
}