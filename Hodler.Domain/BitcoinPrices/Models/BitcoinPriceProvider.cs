using System.ComponentModel;

namespace Hodler.Domain.BitcoinPrices.Models;

public enum BitcoinPriceProvider
{
    [Description("Dia")] Dia = 1,
    [Description("BitPandaTicker")] BitPandaTicker = 2,
}