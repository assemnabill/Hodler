using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public class PortfolioSummary
{

    public FiatAmount FiatNetInvested { get; }
    public BitcoinAmount TotalBitcoin { get; }
    public FiatAmount CurrentBitcoinPrice { get; }
    public FiatAmount PortfolioValue { get; }
    public FiatAmount FiatReturnOnInvestment { get; }
    public double FiatReturnOnInvestmentPercentage { get; }
    public FiatAmount AverageBitcoinPrice { get; }
    public FiatAmount TaxFreeFiatReturnOnInvestment { get; }
    public double TaxFreeFiatReturnOnInvestmentPercentage { get; }

    public PortfolioSummary(
        FiatAmount fiatNetInvested,
        BitcoinAmount totalBitcoin, 
        FiatAmount currentBitcoinPrice, 
        FiatAmount portfolioValue,
        FiatAmount fiatReturnOnInvestment, 
        double fiatReturnOnInvestmentPercentage, 
        FiatAmount averageBitcoinPrice, 
        FiatAmount taxFreeFiatReturnOnInvestment,
        double taxFreeFiatReturnOnInvestmentPercentage)
    {
        FiatNetInvested = fiatNetInvested;
        TotalBitcoin = totalBitcoin;
        CurrentBitcoinPrice = currentBitcoinPrice;
        PortfolioValue = portfolioValue;
        FiatReturnOnInvestment = fiatReturnOnInvestment;
        FiatReturnOnInvestmentPercentage = fiatReturnOnInvestmentPercentage;
        AverageBitcoinPrice = averageBitcoinPrice;
        TaxFreeFiatReturnOnInvestment = taxFreeFiatReturnOnInvestment;
        TaxFreeFiatReturnOnInvestmentPercentage = taxFreeFiatReturnOnInvestmentPercentage;
    }
}