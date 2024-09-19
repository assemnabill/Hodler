using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Domain.Portfolio.Models;
using Mapster;
using TransactionType = Hodler.Contracts.Portfolio.TransactionType;

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

        config
            .NewConfig<TransactionInfo, TransactionInfoDto>()
            .MapWith((src) =>
                new TransactionInfoDto(
                    (TransactionType)src.Type,
                    src.FiatAmount.Amount,
                    src.BtcAmount.Amount,
                    src.MarketPrice,
                    src.Timestamp
                ));
    }
}