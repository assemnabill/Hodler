
using System.ComponentModel;

namespace Hodler.Domain.CryptoExchange.Models;

public enum CryptoExchangeNames
{
    [Description("BitPanda")] BitPanda = 1,
    [Description("Kraken")] Kraken = 2
}
