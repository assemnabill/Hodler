namespace Hodler.Contracts.WalletConnections;

public record ConnectedWalletsDto(IReadOnlyCollection<ConnectedWalletDto> Wallets);