using Hodler.Contracts.Shared;

namespace Hodler.Contracts.Portfolios.PortfolioValueChart;

public class ChartSpotDto
{
    public DateOnly Date { get; set; }
    public FiatAmountDto PortfolioValue { get; set; }
}