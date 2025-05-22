using System.Text.RegularExpressions;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public partial class BitcoinAddress
{
    public string Value { get; }

    public BitcoinAddress(string bitcoinAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bitcoinAddress, nameof(bitcoinAddress));

        if (!BitcoinAddressRegex().IsMatch(bitcoinAddress))
            throw new ArgumentException("Invalid Bitcoin address format");

        Value = bitcoinAddress;
    }

    [GeneratedRegex("^(bc1|[13])[a-zA-HJ-NP-Z0-9]{25,39}$")]
    private static partial Regex BitcoinAddressRegex();
}