using Hodler.Contracts.Portfolios.PortfolioValueChart;
using Hodler.Domain.Portfolios.Models;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class ChartCandleRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ChartCandle, ChartCandleDto>()
            .Map(x => x.Date, x => x.Date)
            .Map(x => x.PortfolioValue, x => x.PortfolioValue.Amount);
    }
}