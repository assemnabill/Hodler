namespace Hodler.Domain.Transactions.Models;

public class TransactionsSummaryReport
{
    public double NetInvestedFiat { get; }
    public double TotalBtcInvestment { get; }
    public double CurrentBtcPrice { get; }
    public double CurrentValue { get; }
    public double TotalProfitFiat { get; }
    public double TotalProfitPercentage { get; }
    public double AverageBtcPrice { get; }
    public double TaxFreeTotalBtcInvestment { get; }
    public double TaxFreeProfit { get; }

    public TransactionsSummaryReport(
        double netInvestedFiat,
        double totalBtcInvestment, 
        double currentBtcPrice, 
        double currentValue,
        double totalProfitFiat, 
        double totalProfitPercentage, 
        double averageBtcPrice, 
        double taxFreeTotalBtcInvestment,
        double taxFreeProfit)
    {
        NetInvestedFiat = netInvestedFiat;
        TotalBtcInvestment = totalBtcInvestment;
        CurrentBtcPrice = currentBtcPrice;
        CurrentValue = currentValue;
        TotalProfitFiat = totalProfitFiat;
        TotalProfitPercentage = totalProfitPercentage;
        AverageBtcPrice = averageBtcPrice;
        TaxFreeTotalBtcInvestment = taxFreeTotalBtcInvestment;
        TaxFreeProfit = taxFreeProfit;
    }
}