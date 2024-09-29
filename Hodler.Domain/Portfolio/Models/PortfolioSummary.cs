using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public class PortfolioSummary
{

    public FiatAmount NetInvestedFiat { get; }
    public BitcoinAmount TotalBtcInvestment { get; }
    public FiatAmount CurrentBtcPrice { get; }
    public FiatAmount CurrentValue { get; }
    public FiatAmount ProfitLossInFiat { get; }
    public double ProfitLossInFiatPercentage { get; }
    public FiatAmount AverageBtcPrice { get; }
    public FiatAmount TaxFreeBtcInvestment { get; }
    public double TaxFreeProfitPercentage { get; }

    public PortfolioSummary(
        FiatAmount netInvestedFiat,
        BitcoinAmount totalBtcInvestment, 
        FiatAmount currentBtcPrice, 
        FiatAmount currentValue,
        FiatAmount profitLossInFiat, 
        double profitLossInFiatPercentage, 
        FiatAmount averageBtcPrice, 
        FiatAmount taxFreeBtcInvestment,
        double taxFreeProfitPercentage)
    {
        NetInvestedFiat = netInvestedFiat;
        TotalBtcInvestment = totalBtcInvestment;
        CurrentBtcPrice = currentBtcPrice;
        CurrentValue = currentValue;
        ProfitLossInFiat = profitLossInFiat;
        ProfitLossInFiatPercentage = profitLossInFiatPercentage;
        AverageBtcPrice = averageBtcPrice;
        TaxFreeBtcInvestment = taxFreeBtcInvestment;
        TaxFreeProfitPercentage = taxFreeProfitPercentage;
    }
}