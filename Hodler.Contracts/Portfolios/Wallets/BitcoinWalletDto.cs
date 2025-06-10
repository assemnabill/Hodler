namespace Hodler.Contracts.Portfolios.Wallets;

public record BitcoinWalletDto(
    Guid Id,
    string WalletName,
    string Address,
    decimal Balance,
    DateTimeOffset ConnectedDate,
    DateTimeOffset? LastSynced,
    IReadOnlyCollection<BlockchainTransactionDto> Transactions
);