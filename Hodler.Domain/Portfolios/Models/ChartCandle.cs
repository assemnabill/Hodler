using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class ChartCandle
{
    public ChartCandle(DateOnly date, FiatAmount portfolioValue)
    {
        Date = date;
        PortfolioValue = portfolioValue;
    }

    public DateOnly Date { get; }
    public FiatAmount PortfolioValue { get; }
}