using Hodler.Domain.Portfolios.Services;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public interface IBitcoinWallets : IReadOnlyCollection<IBitcoinWallet>
{
    Task<IBitcoinWallets> ConnectWalletAsync(
        PortfolioId id,
        BitcoinAddress address,
        string walletName,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken
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