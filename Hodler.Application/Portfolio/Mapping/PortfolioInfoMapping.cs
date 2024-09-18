using Hodler.Domain.Portfolio.Models;
using Mapster;

namespace Hodler.Application.Portfolio.Mapping;

public class PortfolioInfoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IPortfolio, PortfolioInfo>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(
                dest => dest.Transactions,
                src => src.Transactions
                    .Select(x => x.Adapt<TransactionInfo>())
            );
    }
}