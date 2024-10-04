using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Mapster;

namespace Hodler.Application.Portfolio.Mapping;

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
                src.transactionInfo.Type,
                src.transactionInfo.FiatAmount,
                src.transactionInfo.BtcAmount,
                src.transactionInfo.MarketPrice,
                src.transactionInfo.Timestamp,
                src.transactionInfo.CryptoExchange
            ));

        config
            .NewConfig<FiatAmount, decimal>()
            .MapWith(src => src.Amount);
    }
}