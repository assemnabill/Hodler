using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public class BitcoinWalletId : PrimitiveWrapper<Guid, BitcoinWalletId>
{
    public BitcoinWalletId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid {nameof(BitcoinWalletId)}");
    }
}