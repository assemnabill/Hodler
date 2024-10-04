using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Web.Components.Shared.Services;

namespace Hodler.Web.Services.Portfolio;

public class PortfolioServiceViewService : IPortfolioServiceViewService
{
    private readonly ISessionService _sessionService;
    private readonly IHodlerApiClient _hodlerApi;

    public event EventHandler<EventArgs> PortfolioSummaryChanged;
    public PortfolioSummaryDto? PortfolioSummary { get; private set; }

    public PortfolioServiceViewService(ISessionService sessionService, IHodlerApiClient hodlerApi)
    {
        _sessionService = sessionService;
        _hodlerApi = hodlerApi;
    }

    public async Task InitPortfolioSummaryAsync()
    {
        var userId = _sessionService.GetCurrentUserId();
        PortfolioSummary = await _hodlerApi.GetPortfolioSummaryAsync(userId);
        NotifyPortfolioSummaryChanged(PortfolioSummary, EventArgs.Empty);
    }


    public void NotifyPortfolioSummaryChanged(object? sender, EventArgs e)
        => PortfolioSummaryChanged?.Invoke(sender, e);

    // public void HandleBitcoinPriceChanged(BitcoinPricePerCurrencyCatalogDto bitcoinPrice)
    // {
    //     // TODO: GET FROM SETTINGS
    //     var userCurrency = fiatcurrency.Euro;
    //     var currentBitcoinPrice = bitcoinPrice.Catalog.FirstOrDefault(x => x.FiatCurrency == userCurrency);
    //
    //     if (currentBitcoinPrice is null || _portfolioSummaryDto is null)
    //     {
    //         return;
    //     }
    //
    //     var totalBtcInvestment = _portfolioSummaryDto.TotalBitcoin;
    //     var portfolioValue = new FiatAmountDto
    //     {
    //         FiatCurrency = currentBitcoinPrice.FiatCurrency,
    //         Amount = totalBtcInvestment * currentBitcoinPrice.Amount
    //     };
    //     var netInvestedFiat = _portfolioSummaryDto.FiatNetInvested;
    //     var fiatReturnOnInvestment = new FiatAmountDto
    //     {
    //         FiatCurrency = netInvestedFiat.FiatCurrency,
    //         Amount = portfolioValue.Amount - netInvestedFiat.Amount
    //     };
    //     var fiatReturnOnInvestmentPercentage =
    //         Convert.ToDouble(fiatReturnOnInvestment.Amount / netInvestedFiat.Amount * 100);
    //     var averageBtcPrice = _portfolioSummaryDto.AverageBitcoinPrice;
    //     var taxFreeBtcInvestment = _portfolioSummaryDto.TaxFreeFiatReturnOnInvestment;
    //     var taxFreeProfitPercentage = _portfolioSummaryDto.TaxFreeFiatReturnOnInvestmentPercentage;
    //
    //     _portfolioSummaryDto = new PortfolioSummaryDto(
    //         netInvestedFiat,
    //         totalBtcInvestment,
    //         currentBitcoinPrice,
    //         portfolioValue, //
    //         fiatReturnOnInvestment, //
    //         fiatReturnOnInvestmentPercentage, //
    //         averageBtcPrice, //
    //         taxFreeBtcInvestment,
    //         taxFreeProfitPercentage
    //     );
    // }
}