using Corz.DomainDriven.Abstractions.Failures;
using BitcoinAddress = Hodler.Domain.Portfolios.Models.BitcoinWallets.BitcoinAddress;

namespace Hodler.Domain.Portfolios.Failures;

public class BitcoinWalletAlreadyExistsFailure : Failure
{
    public BitcoinAddress Address { get; }

    public BitcoinWalletAlreadyExistsFailure(BitcoinAddress address)
    {
        Address = address;
    }
}