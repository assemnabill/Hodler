using System.Net.Http.Json;
using Hodler.Domain.Shared.Ports.DiaDataApi;

namespace Hodler.Integration.ExternalApis.PriceFeed;

public class DiaDataApiClient : IDiaDataApiClient
{
    private readonly HttpClient _httpClient;

    public DiaDataApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    const string BitCoinAssetQuotationEndpoint =
        "https://api.diadata.org/v1/assetQuotation/Bitcoin/0x0000000000000000000000000000000000000000";


    public async Task<AssetQuotation?> GetBitcoinAssetQuotationAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetFromJsonAsync<AssetQuotation>(BitCoinAssetQuotationEndpoint, cancellationToken);
    }
}