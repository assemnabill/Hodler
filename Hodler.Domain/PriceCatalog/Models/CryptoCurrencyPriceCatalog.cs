using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Models;

public class CryptoCurrencyPriceCatalog
    : Dictionary<CryptoCurrency, IFiatAmountCatalog>, IPriceCatalog<CryptoCurrency>
{
}