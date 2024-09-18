using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Domain.Portfolio.Models;
using Mapster;

namespace Hodler.ApiService.UserScope.Portfolio;

public class PortfolioInfoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<PortfolioInfo, PortfolioInfoDto>()
            .MapWith((src) =>
                new PortfolioInfoDto(
                    src.Id.Value,
                    src.Transactions.Select(x => x.Adapt<TransactionInfoDto>()).ToList()
                ));
    }
}