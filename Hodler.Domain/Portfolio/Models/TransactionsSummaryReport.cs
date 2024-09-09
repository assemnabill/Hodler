namespace Hodler.Domain.Portfolio.Models;

public class TransactionsSummaryReport
{
    public double NetInvestedFiat { get; }
    public double TotalBtcInvestment { get; }
    public double CurrentBtcPrice { get; }
    public double CurrentValue { get; }
    public double ProfitLossInFiat { get; }
    public double ProfitLossInFiatPercentage { get; }
    public double AverageBtcPrice { get; }
    public double TaxFreeBtcInvestment { get; }
    public double TaxFreeProfitPercentage { get; }

    public TransactionsSummaryReport(
        double netInvestedFiat,
        double totalBtcInvestment, 
        double currentBtcPrice, 
        double currentValue,
        double profitLossInFiat, 
        double profitLossInFiatPercentage, 
        double averageBtcPrice, 
        double taxFreeBtcInvestment,
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