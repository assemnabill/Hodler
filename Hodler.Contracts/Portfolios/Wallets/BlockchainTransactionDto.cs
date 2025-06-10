using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Contracts.Portfolios.Wallets;

public record BlockchainTransactionDto(
    decimal Amount,
    string TransactionHash,
    FiatAmountDto MarketPrice,
    FiatAmountDto FiatValue,
    DateTimeOffset Timestamp,
    BlockchainTransactionStatus Status,
    string FromAddress,
    string ToAddress,
    decimal NetworkFee,
    FiatAmountDto FiatFee,
    string? Note,
    TransactionType TransactionType,
    Guid BitcoinWalletId
);