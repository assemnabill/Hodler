using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Portfolios.Failures;

public class BitcoinWalletSyncFailure : Failure
{
    public BitcoinWalletId WalletId { get; }
    public Exception Error { get; }

    public BitcoinWalletSyncFailure(BitcoinWalletId walletId, Exception error)
    {
        WalletId = walletId;
        Error = error;
    }
}