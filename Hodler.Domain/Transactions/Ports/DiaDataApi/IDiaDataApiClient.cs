namespace Hodler.Domain.Transactions.Ports.DiaDataApi;

public interface IDiaDataApiClient
{
    Task<AssetQuotation?> GetBitcoinAssetQuotationAsync(CancellationToken cancellationToken);
}