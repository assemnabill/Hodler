using System.Collections.ObjectModel;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.PriceCatalog.Models;

public class FiatAmountCatalog : ReadOnlyCollection<FiatAmount>, IFiatAmountCatalog
{
    public FiatAmountCatalog(IList<FiatAmount> list) : base(list)
    {
    }

    public FiatAmount GetPrice(FiatCurrency currency) => 
        this.First(fiatAmount => fiatAmount.FiatCurrency.Equals(currency));
}