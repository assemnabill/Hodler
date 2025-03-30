using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class PortfolioValueChartInfo
{
    public IReadOnlyCollection<ChartSpot> Spots { get; }
    public FiatAmount PortfolioValueInFiat { get; }
    public double FiatReturnOnInvestmentPercentage { get; }

    public PortfolioValueChartInfo(IReadOnlyCollection<ChartSpot> spots, FiatAmount portfolioValueInFiat, double fiatReturnOnInvestmentPercentage)
    {
        Spots = spots;
        PortfolioValueInFiat = portfolioValueInFiat;
        FiatReturnOnInvestmentPercentage = fiatReturnOnInvestmentPercentage;
    }
}