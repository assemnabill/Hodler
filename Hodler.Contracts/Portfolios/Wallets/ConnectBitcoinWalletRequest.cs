namespace Hodler.Contracts.Portfolios.Wallets;

public record ConnectBitcoinWalletRequest(
    string Name,
    string Address
);