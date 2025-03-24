using Hodler.Contracts.Shared;

namespace Hodler.Contracts.PriceCatalogs;

public class BitcoinPricePerCurrencyCatalogDto
{
    public IEnumerable<FiatAmountDto> Catalog { get; set; }
}