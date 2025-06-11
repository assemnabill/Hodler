using Hodler.Domain.Portfolios.Services;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public interface IBitcoinWallets : IReadOnlyCollection<IBitcoinWallet>
{
    Task<IBitcoinWallets> ConnectWalletAsync(
        PortfolioId id,
        BitcoinAddress address,
        WalletName walletName,
        IBitcoinBlockchainService blockchainService,
        WalletAvatar? avatar = null,
        CancellationToken cancellationToken = default
    );

    Task<IBitcoinWallets> SyncWalletAsync(
        BitcoinWalletId walletId,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    );

    IBitcoinWallets Disconnect(BitcoinWalletId walletId);

    IBitcoinWallet? FindById(BitcoinWalletId walletId);

    bool AlreadyConnected(BitcoinAddress address);

    bool AlreadyConnected(BitcoinWalletId walletId);
}