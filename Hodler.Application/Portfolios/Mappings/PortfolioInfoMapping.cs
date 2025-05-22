using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Mapster;

namespace Hodler.Application.Portfolios.Mappings;

public class PortfolioInfoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<IPortfolio, PortfolioInfo>()
            .MapWith(src => new PortfolioInfo(
                src.Id,
                src.Transactions.Select(x => x.Adapt<TransactionInfo>()).ToList()
            ));

        config
            .NewConfig<
                (PortfolioId portfolioId, TransactionInfo transactionInfo),
                Transaction>()
            .MapWith(src => new Transaction(
                src.portfolioId,
                src.transactionInfo.Id,
                src.transactionInfo.Type,
                src.transactionInfo.FiatAmount,
                src.transactionInfo.BtcAmount,
                src.transactionInfo.Timestamp,
                src.transactionInfo.MarketPrice,
                src.transactionInfo.TransactionSource,
                null
            ));

        config
            .NewConfig<FiatAmount, decimal>()
            .MapWith(src => src.Amount);
    }
}