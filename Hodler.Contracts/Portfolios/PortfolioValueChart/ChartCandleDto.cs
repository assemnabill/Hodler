namespace Hodler.Contracts.Portfolios.PortfolioValueChart;

public class ChartCandleDto
{
    public DateOnly Date { get; set; }
    public double PortfolioValue { get; set; }
}