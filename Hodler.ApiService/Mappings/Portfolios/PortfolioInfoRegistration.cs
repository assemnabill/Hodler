using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.Transactions;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class PortfolioInfoRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<PortfolioInfo, PortfolioTransactionsDto>()
            .MapWith((src) =>
                new PortfolioTransactionsDto(
                    src.Id.Value,
                    src.Transactions.Select(x => x.Adapt<TransactionInfoDto>()).ToList()
                ));

        config
            .NewConfig<TransactionInfo, TransactionInfoDto>()
            .MapWith((src) =>
                new TransactionInfoDto(
                    src.Id.Value,
                    src.Type,
                    src.FiatAmount.Adapt<FiatAmountDto>(),
                    src.BtcAmount.Amount,
                    src.MarketPrice.Adapt<FiatAmountDto>(),
                    src.Timestamp
                ));

    }
}