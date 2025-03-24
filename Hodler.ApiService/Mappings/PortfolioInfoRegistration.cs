using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Domain.Portfolios.Models;
using Mapster;
using TransactionType = Hodler.Contracts.Portfolios.TransactionType;

namespace Hodler.ApiService.Mappings;

public class PortfolioInfoRegistration : IRegister
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