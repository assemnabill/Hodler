using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Portfolios.PortfolioValueChart;

public class PortfolioValueChartDto
{
    public IReadOnlyCollection<ChartSpotDto> Spots { get; set; }
    public FiatAmountDto PortfolioValueInFiat { get; set; }
    public double FiatReturnOnInvestmentPercentage { get; set; }
}