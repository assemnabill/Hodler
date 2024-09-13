using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using Mapster;

namespace Hodler.Integration.Repositories.Portfolio.Mappings;

public class PortfolioMappingRegisteration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Entities.Portfolio, IPortfolio>()
            .MapWith((portfolio => new Domain.Portfolio.Models.Portfolio(
                new Transactions(portfolio.Transactions.Select(x => x.Adapt<Entities.Transaction, Transaction>())),
                new UserId(portfolio.PortfolioId)
            )));

        config
            .NewConfig<IPortfolio, Entities.Portfolio>()
            .Map(dest => dest.PortfolioId, src => src.PortfolioId.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.Transactions,
                src => src.Transactions.Select(x => x.Adapt<Transaction, Entities.Transaction>()).ToList());

        config
            .NewConfig<Entities.Transaction, Transaction>()
            .MapWith(transaction => new Transaction(
                transaction.Id,
                (TransactionType)transaction.Type,
                FiatCurrency.AsEnumerable().FirstOrDefault(x => x.Id == transaction.FiatCurrency)!,
                transaction.FiatAmount,
                transaction.BtcAmount,
                transaction.MarketPrice,
                transaction.Timestamp,
                CryptoExchange.AsEnumerable().FirstOrDefault(x => x.Id == transaction.CryptoExchange)
            ));

        config
            .NewConfig<Transaction, Entities.Transaction>()
            .Map(dest => dest.Id, src => src.TransactionId)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.FiatAmount, src => src.FiatAmount)
            .Map(dest => dest.BtcAmount, src => src.BtcAmount)
            .Map(dest => dest.MarketPrice, src => src.MarketPrice)
            .Map(dest => dest.Timestamp, src => src.Timestamp);
    }
}