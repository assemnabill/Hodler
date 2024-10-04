using Hodler.Contracts.Shared;

namespace Hodler.Contracts.PriceCatalog;

public class BitcoinPricePerCurrencyCatalogDto
{
    public IEnumerable<FiatAmountDto> Catalog { get; set; }
}