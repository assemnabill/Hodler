using System.ComponentModel;

namespace Hodler.Domain.PriceCatalogs.Models;

public enum BitcoinPriceProvider
{
    [Description("Dia")] Dia = 1,
    [Description("BitPandaTicker")] BitPandaTicker = 2,
}