namespace Hodler.Contracts.WalletConnections;

public record ConnectBitcoinWalletRequest(
    string Name,
    string Address,
    int ChainId
);