using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using Mapster;

namespace Hodler.Integration.Repositories.Portfolio.Mappings;

public class PortfolioMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Entities.Portfolio, IPortfolio>()
            .MapWith((portfolio => new Domain.Portfolio.Models.Portfolio(
                new PortfolioId(portfolio.PortfolioId),
                new Transactions(portfolio.Transactions.Select(x => x.Adapt<Entities.Transaction, Transaction>())),
                new UserId(portfolio.PortfolioId)
            )));

        config
            .NewConfig<IPortfolio, Entities.Portfolio>()
            .Map(dest => dest.PortfolioId, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.Transactions,
                src => src.Transactions.Select(x => x.Adapt<Transaction, Entities.Transaction>()).ToList());

        config
            .NewConfig<Entities.Transaction, Transaction>()
            .MapWith(transaction => new Transaction(
                (TransactionType)transaction.Type,
                new FiatAmount(transaction.FiatAmount,
                    FiatCurrency.AsEnumerable().FirstOrDefault(x => x.Id == transaction.FiatCurrency)!),
                new BitcoinAmount(transaction.BtcAmount),
                transaction.MarketPrice,
                transaction.Timestamp,
                CryptoExchange.AsEnumerable().FirstOrDefault(x => x.Id == transaction.CryptoExchange)
            ));

        config
            .NewConfig<Transaction, Entities.Transaction>()
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.FiatAmount, src => src.FiatAmount.Amount)
            .Map(dest => dest.FiatCurrency, src => src.FiatAmount.FiatCurrency.Id)
            .Map(dest => dest.BtcAmount, src => src.BtcAmount.Amount)
            .Map(dest => dest.MarketPrice, src => src.MarketPrice)
            .Map(dest => dest.Timestamp, src => src.Timestamp);
    }
}