using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Models;
using BitcoinAddress = Hodler.Domain.Portfolios.Models.BitcoinWallets.BitcoinAddress;

namespace Hodler.Domain.Portfolios.Services;

public interface IBitcoinBlockchainService
{
    Task<IReadOnlyCollection<BlockchainTransaction>> GetTransactionsAsync(
        IBitcoinWallet wallet,
        CancellationToken cancellationToken = default
    );

    Task<BitcoinAmount> GetCurrentBalanceAsync(
        BitcoinAddress walletAddress,
        CancellationToken cancellationToken = default
    );
}