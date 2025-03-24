namespace Hodler.Domain.PriceCatalogs.Models;

public interface IPriceCatalog<TAsset> : IDictionary<TAsset, IFiatAmountCatalog>
{
}