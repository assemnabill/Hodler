using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Portfolios.Failures;

public class BitcoinWalletIsNotConnectedFailure : Failure
{
    public BitcoinWalletId WalletId { get; }

    public BitcoinWalletIsNotConnectedFailure(BitcoinWalletId walletId)
    {
        WalletId = walletId;
    }
}