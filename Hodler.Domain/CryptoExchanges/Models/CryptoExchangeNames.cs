
using System.ComponentModel;

namespace Hodler.Domain.CryptoExchanges.Models;

public enum CryptoExchangeNames
{
    [Description("BitPanda")] BitPanda = 1,
    [Description("Kraken")] Kraken = 2
}
