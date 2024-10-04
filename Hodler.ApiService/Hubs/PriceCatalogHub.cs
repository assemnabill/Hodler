using Hodler.Contracts.PriceCatalog;
using Hodler.Domain.PriceCatalog.Services;
using Mapster;
using Microsoft.AspNetCore.SignalR;

namespace Hodler.ApiService.Hubs;

public class PriceCatalogHub : Hub
{
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;

    public PriceCatalogHub(ICurrentBitcoinPriceProvider currentBitcoinPriceProvider)
    {
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
    }

    public async Task BitcoinPriceCatalog()
    {
        var currentPriceCatalog = await _currentBitcoinPriceProvider.GetBitcoinPriceCatalogAsync(default);
        var dto = currentPriceCatalog.Adapt<BitcoinPricePerCurrencyCatalogDto>();

        await Clients.All.SendAsync("BitcoinPriceChanged", dto);
    }
}