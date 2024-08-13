using Hodler.Domain.Transactions.Ports.DiaDataApi;

namespace Hodler.Domain.Transactions.Services;

public class CurrentPriceProvider : ICurrentPriceProvider
{
    private readonly IDiaDataApiClient _diaDataApiClient;

    public CurrentPriceProvider(IDiaDataApiClient diaDataApiClient)
    {
        _diaDataApiClient = diaDataApiClient;
    }

    public async Task<double> GetCurrentPriceAsync(CancellationToken cancellationToken)
    {
        var qoutation = await _diaDataApiClient.GetBitcoinAssetQuotationAsync(cancellationToken);

        return qoutation!.Price;
    }
}