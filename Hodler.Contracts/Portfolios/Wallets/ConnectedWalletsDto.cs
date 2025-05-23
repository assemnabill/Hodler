namespace Hodler.Contracts.Portfolios.Wallets;

public record ConnectedWalletsDto(IReadOnlyCollection<BitcoinWalletDto> Wallets);