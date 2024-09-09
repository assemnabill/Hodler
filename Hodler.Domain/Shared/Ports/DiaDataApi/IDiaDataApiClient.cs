namespace Hodler.Domain.Shared.Ports.DiaDataApi;

public interface IDiaDataApiClient
{
    Task<AssetQuotation?> GetBitcoinAssetQuotationAsync(CancellationToken cancellationToken);
}