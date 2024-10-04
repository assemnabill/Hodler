using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Contracts.PriceCatalog;
using Hodler.Contracts.Shared;
using Hodler.Web.Components.Shared.Services;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace Hodler.Web.Services.Portfolio;

public class PortfolioServiceViewService : IPortfolioServiceViewService
{
    private readonly ISessionService _sessionService;
    private readonly IHodlerApiClient _hodlerApi;
    private readonly IHttpMessageHandlerFactory _httpMessageHandlerFactory;
    private HubConnection? _hubConnection;
    public event EventHandler<EventArgs> PortfolioSummaryChanged;
    public PortfolioSummaryDto? PortfolioSummary { get; private set; }

    public PortfolioServiceViewService(
        ISessionService sessionService,
        IHodlerApiClient hodlerApi,
        IHttpMessageHandlerFactory httpMessageHandlerFactory)
    {
        _sessionService = sessionService;
        _hodlerApi = hodlerApi;
        _httpMessageHandlerFactory = httpMessageHandlerFactory;

        PortfolioSummary = null;
    }

    public async Task InitPortfolioSummaryAsync()
    {
        var userId = _sessionService.GetCurrentUserId();
        PortfolioSummary = await _hodlerApi.GetPortfolioSummaryAsync(userId);
        NotifyPortfolioSummaryChanged(PortfolioSummary, EventArgs.Empty);

        await StartHubConnection();
    }

    private async Task StartHubConnection()
    {
        var url = $"{_hodlerApi.BaseUri}priceCatalog";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url, (Action<HttpConnectionOptions>)ConfigureHttpConnection)
            .Build();

        _hubConnection.On<BitcoinPricePerCurrencyCatalogDto>(
            "BitcoinPriceChanged",
            HandleBitcoinPriceChanged
        );

        await _hubConnection.StartAsync();
    }

    private void ConfigureHttpConnection(HttpConnectionOptions options) =>
        options.HttpMessageHandlerFactory = _ => _httpMessageHandlerFactory.CreateHandler();

    private void NotifyPortfolioSummaryChanged(object? sender, EventArgs e)
        => PortfolioSummaryChanged?.Invoke(sender, e);

    private void HandleBitcoinPriceChanged(BitcoinPricePerCurrencyCatalogDto bitcoinPrice)
    {
        // TODO: GET FROM SETTINGS
        var userCurrency = FiatCurrency.Euro;
        var currentBitcoinPrice = bitcoinPrice.Catalog.FirstOrDefault(x => x.FiatCurrency == userCurrency);

        if (currentBitcoinPrice is null || PortfolioSummary is null)
        {
            return;
        }

        var totalBtcInvestment = PortfolioSummary.TotalBitcoin;
        var portfolioValue = new FiatAmountDto
        {
            FiatCurrency = currentBitcoinPrice.FiatCurrency,
            Amount = totalBtcInvestment * currentBitcoinPrice.Amount
        };
        var netInvestedFiat = PortfolioSummary.FiatNetInvested;
        var fiatReturnOnInvestment = new FiatAmountDto
        {
            FiatCurrency = netInvestedFiat.FiatCurrency,
            Amount = portfolioValue.Amount - netInvestedFiat.Amount
        };
        var fiatReturnOnInvestmentPercentage =
            Convert.ToDouble(fiatReturnOnInvestment.Amount / netInvestedFiat.Amount * 100);
        var averageBtcPrice = PortfolioSummary.AverageBitcoinPrice;
        var taxFreeBtcInvestment = PortfolioSummary.TaxFreeFiatReturnOnInvestment;
        var taxFreeProfitPercentage = PortfolioSummary.TaxFreeFiatReturnOnInvestmentPercentage;

        PortfolioSummary = new PortfolioSummaryDto(
            netInvestedFiat,
            totalBtcInvestment,
            currentBitcoinPrice,
            portfolioValue, //
            fiatReturnOnInvestment, //
            fiatReturnOnInvestmentPercentage, //
            averageBtcPrice, //
            taxFreeBtcInvestment,
            taxFreeProfitPercentage
        );

        NotifyPortfolioSummaryChanged(PortfolioSummary, EventArgs.Empty);
    }
}