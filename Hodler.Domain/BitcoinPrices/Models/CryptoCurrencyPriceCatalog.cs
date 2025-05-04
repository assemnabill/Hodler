using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Models;

public class CryptoCurrencyPriceCatalog
    : Dictionary<CryptoCurrency, IFiatAmountCatalog>, IPriceCatalog<CryptoCurrency>
{
}