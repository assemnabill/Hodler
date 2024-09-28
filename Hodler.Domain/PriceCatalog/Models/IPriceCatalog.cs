namespace Hodler.Domain.PriceCatalog.Models;

public interface IPriceCatalog<TAsset> : IDictionary<TAsset, IFiatAmountCatalog>
{
}