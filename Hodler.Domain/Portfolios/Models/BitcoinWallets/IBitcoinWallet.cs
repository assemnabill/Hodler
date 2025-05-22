using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public interface IBitcoinWallet
{
    BitcoinWalletId Id { get; }
    PortfolioId PortfolioId { get; }
    BitcoinAddress Address { get; }
    string WalletName { get; }
    BlockchainNetwork Network { get; }
    DateTimeOffset ConnectedDate { get; }
    DateTimeOffset? LastSynced { get; }
    BitcoinAmount Balance { get; }

    Task<IBitcoinWallet> UpdateBalanceAsync(
        IBitcoinBlockchainService bitcoinBlockchainService,
        CancellationToken cancellationToken = default
    );
}