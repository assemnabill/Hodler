namespace Hodler.Domain.BitcoinPrices.Models;

public interface IPriceCatalog<TAsset> : IDictionary<TAsset, IFiatAmountCatalog>
{
}