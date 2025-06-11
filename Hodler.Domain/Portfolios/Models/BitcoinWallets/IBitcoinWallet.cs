using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public interface IBitcoinWallet
{
    BitcoinWalletId Id { get; }
    PortfolioId PortfolioId { get; }
    BitcoinAddress Address { get; }
    WalletName WalletName { get; }
    WalletAvatar Avatar { get; }
    BlockchainNetwork Network { get; }
    DateTimeOffset ConnectedDate { get; }
    DateTimeOffset? LastSynced { get; }
    BitcoinAmount Balance { get; }
    IReadOnlyCollection<BlockchainTransaction> Transactions { get; }

    Task<IBitcoinWallet> SyncAsync(
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    );
}