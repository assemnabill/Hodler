namespace Hodler.Application.Portfolios.Queries.ConnectedWallets;

public record BitcoinWalletDto(
    Guid Id,
    string Address,
    string WalletName,
    DateTimeOffset ConnectedDate,
    DateTimeOffset? LastSynced,
    decimal Balance
);