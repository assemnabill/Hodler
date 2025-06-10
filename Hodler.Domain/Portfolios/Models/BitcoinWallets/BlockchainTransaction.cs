using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public record BlockchainTransaction(
    BitcoinAmount NetBitcoin,
    TransactionHash TransactionHash,
    FiatAmount MarketPrice,
    FiatAmount FiatValue,
    DateTimeOffset Timestamp,
    BlockchainTransactionStatus Status,
    BitcoinAddress FromAddress,
    BitcoinAddress ToAddress,
    BitcoinAmount NetworkFee,
    FiatAmount FiatFee,
    string? Note,
    TransactionType TransactionType,
    PortfolioId PortfolioId,
    BitcoinWalletId BitcoinWalletId
);