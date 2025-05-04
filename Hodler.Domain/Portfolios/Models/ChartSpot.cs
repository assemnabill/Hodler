using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class ChartSpot
{
    public DateOnly Date { get; }
    public FiatAmount PortfolioValue { get; }

    public ChartSpot(DateOnly date, FiatAmount portfolioValue)
    {
        Date = date;
        PortfolioValue = portfolioValue;
    }
}