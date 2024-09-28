using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Models;

public interface IFiatAmountCatalog : IReadOnlyCollection<FiatAmount>
{
    FiatAmount GetPrice(FiatCurrency usDollar);
}