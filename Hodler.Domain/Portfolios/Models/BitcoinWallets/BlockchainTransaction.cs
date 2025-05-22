using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

// todo: complete this with rest of properties in response of address/{address.Value}/txs endpoint
public record BlockchainTransaction(
    BitcoinAmount Amount,
    TransactionHash TransactionHash,
    decimal Price,
    decimal FiatValue,
    DateTimeOffset Timestamp
);