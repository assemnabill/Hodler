using Hodler.Contracts.PriceCatalog;
using Hodler.Domain.PriceCatalog.Ports;
using Hodler.Domain.PriceCatalog.Services;
using Mapster;
using Microsoft.AspNetCore.SignalR;

namespace Hodler.ApiService.Hubs;

public class PriceCatalogBroadcastService : BackgroundService
{
    private static readonly TimeSpan UpdateInterval = TimeSpan.FromSeconds(5);
    private readonly IHubContext<PriceCatalogHub> _hubContext;
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;

    public PriceCatalogBroadcastService(
        IHubContext<PriceCatalogHub> hubContext,
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider
    )
    {
        _hubContext = hubContext;
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var periodicTimer = new PeriodicTimer(UpdateInterval);

        while (!stoppingToken.IsCancellationRequested
               && await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            var priceCatalog = await _currentBitcoinPriceProvider.GetBitcoinPriceCatalogAsync(stoppingToken);
            var dto = priceCatalog.Adapt<BitcoinPricePerCurrencyCatalogDto>();
            
            await _hubContext.Clients.All
                .SendAsync("BitcoinPriceChanged", dto, cancellationToken: stoppingToken);
        }
    }
}