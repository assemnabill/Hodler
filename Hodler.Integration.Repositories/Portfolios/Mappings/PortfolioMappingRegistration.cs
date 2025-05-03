using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Mapster;
using Portfolio = Hodler.Integration.Repositories.Portfolios.Entities.Portfolio;
using Transaction = Hodler.Integration.Repositories.Portfolios.Entities.Transaction;

namespace Hodler.Integration.Repositories.Portfolios.Mappings;

public class PortfolioMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Portfolio, IPortfolio>()
            .MapWith(portfolio => new Domain.Portfolios.Models.Portfolio(
                new PortfolioId(portfolio.PortfolioId),
                new Transactions(portfolio.Transactions.Select(x =>
                    x.Adapt<Transaction, Domain.Portfolios.Models.Transaction>())),
                new UserId(Guid.Parse(portfolio.UserId))
            ));

        config
            .NewConfig<IPortfolio, Portfolio>()
            .Map(dest => dest.PortfolioId, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.Transactions,
                src => src.Transactions.Select(x => x.Adapt<Domain.Portfolios.Models.Transaction, Transaction>())
                    .ToList());

        config
            .NewConfig<Transaction, Domain.Portfolios.Models.Transaction>()
            .MapWith(transaction => new Domain.Portfolios.Models.Transaction(
                new PortfolioId(transaction.PortfolioId),
                new TransactionId(transaction.TransactionId),
                (TransactionType)transaction.Type,
                new FiatAmount(transaction.FiatAmount, FiatCurrency.GetById(transaction.FiatCurrency)!),
                new BitcoinAmount(transaction.BtcAmount),
                transaction.Timestamp.ToUniversalTime(),
                (CryptoExchangeName)transaction.CryptoExchange
            ));

        config
            .NewConfig<Domain.Portfolios.Models.Transaction, Transaction>()
            .Map(dest => dest.Type, src => (int)src.Type)
            .Map(dest => dest.PortfolioId, src => src.PortfolioId.Value)
            .Map(dest => dest.FiatAmount, src => src.FiatAmount.Amount)
            .Map(dest => dest.FiatCurrency, src => src.FiatAmount.FiatCurrency.Id)
            .Map(dest => dest.BtcAmount, src => src.BtcAmount.Amount)
            .Map(dest => dest.MarketPrice, src => src.MarketPrice)
            .Map(dest => dest.Timestamp, src => src.Timestamp.UtcDateTime)
            .Map(dest => dest.CryptoExchange, src => src.CryptoExchange);
    }
}