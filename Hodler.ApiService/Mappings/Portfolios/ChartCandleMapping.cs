using Hodler.Contracts.Portfolios.PortfolioValueChart;
using Hodler.Domain.Portfolios.Models;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class ChartCandleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ChartSpot, ChartSpotDto>()
            .Map(x => x.Date, x => x.Date)
            .Map(x => x.PortfolioValue, x => x.PortfolioValue);
    }
}