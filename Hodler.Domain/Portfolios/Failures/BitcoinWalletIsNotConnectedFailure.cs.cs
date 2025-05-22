using Corz.DomainDriven.Abstractions.Failures;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;

namespace Hodler.Domain.Portfolios.Failures;

public class BitcoinWalletIsNotConnectedFailure : Failure
{
    public BitcoinWalletId WalletId { get; }

    public BitcoinWalletIsNotConnectedFailure(BitcoinWalletId walletId)
    {
        WalletId = walletId;
    }
}