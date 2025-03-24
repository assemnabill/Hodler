using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalogs.Models;

public class CryptoCurrencyPriceCatalog
    : Dictionary<CryptoCurrency, IFiatAmountCatalog>, IPriceCatalog<CryptoCurrency>
{
}