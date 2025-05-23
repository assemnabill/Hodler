using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Mapster;
using BitcoinWallet = Hodler.Integration.Repositories.Portfolios.Entities.BitcoinWallet;
using Portfolio = Hodler.Integration.Repositories.Portfolios.Entities.Portfolio;
using Transaction = Hodler.Integration.Repositories.Portfolios.Entities.Transaction;

namespace Hodler.Integration.Repositories.Portfolios.Mappings;

public class PortfolioMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Portfolio, IPortfolio>()
            .MapWith(portfolio => new Domain.Portfolios.Models.Portfolio(
                new PortfolioId(portfolio.PortfolioId),
                new Transactions(portfolio.Transactions.Select(x =>
                    x.Adapt<Transaction, Domain.Portfolios.Models.Transactions.Transaction>())),
                new UserId(Guid.Parse(portfolio.UserId)),
                portfolio.BitcoinWallets
                    .Select(x => x.Adapt<BitcoinWallet, IBitcoinWallet>())
                    .ToList()
            ));

        config
            .NewConfig<IPortfolio, Portfolio>()
            .Map(dest => dest.PortfolioId, src => src.Id.Value)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.Transactions,
                src => src.Transactions
                    .Select(x => x.Adapt<Domain.Portfolios.Models.Transactions.Transaction, Transaction>())
                    .ToList())
            .Map(dest => dest.BitcoinWallets,
                src => src.BitcoinWallets
                    .Select(x => x.Adapt<IBitcoinWallet, BitcoinWallet>())
                    .ToList());

        config
            .NewConfig<Transaction, Domain.Portfolios.Models.Transactions.Transaction>()
            .MapWith(transaction => new Domain.Portfolios.Models.Transactions.Transaction(
                new PortfolioId(transaction.PortfolioId),
                new TransactionId(transaction.TransactionId),
                (TransactionType)transaction.Type,
                new FiatAmount(transaction.FiatAmount, FiatCurrency.GetById(transaction.FiatCurrency)),
                new BitcoinAmount(transaction.BtcAmount),
                transaction.Timestamp.ToUniversalTime(),
                new FiatAmount(transaction.MarketPrice, FiatCurrency.GetById(transaction.FiatCurrency)),
                transaction.SourceIdentifier == null
                    ? null
                    : transaction.SourceType == (int)TransactionSourceType.Wallet
                        ? TransactionSource.FromWallet(
                            new BitcoinWalletId(Guid.Parse(transaction.SourceIdentifier)),
                            transaction.SourceName
                        )
                        : TransactionSource.FromExchange(
                            (CryptoExchangeName)int.Parse(transaction.SourceIdentifier),
                            transaction.SourceName
                        ),
                transaction.Fee
            ));

        config
            .NewConfig<Domain.Portfolios.Models.Transactions.Transaction, Transaction>()
            .Map(dest => dest.Type, src => (int)src.Type)
            .Map(dest => dest.PortfolioId, src => src.PortfolioId.Value)
            .Map(dest => dest.TransactionId, src => src.Id.Value)
            .Map(dest => dest.FiatAmount, src => src.FiatAmount.Amount)
            .Map(dest => dest.FiatCurrency, src => src.FiatAmount.FiatCurrency.Id)
            .Map(dest => dest.BtcAmount, src => src.BtcAmount.Amount)
            .Map(dest => dest.MarketPrice, src => src.MarketPrice)
            .Map(dest => dest.Timestamp, src => src.Timestamp.UtcDateTime)
            .Map(dest => dest.SourceType, src => src.TransactionSource == null ? (int?)null : src.TransactionSource.Type.ToInt())
            .Map(dest => dest.SourceIdentifier, src => src.TransactionSource == null ? null : src.TransactionSource.Identifier)
            .Map(dest => dest.SourceName,
                src => src.TransactionSource == null || src.TransactionSource.Name == null ? null : src.TransactionSource.Name);

        config
            .NewConfig<BitcoinWallet, IBitcoinWallet>()
            .MapWith(wallet => new Domain.Portfolios.Models.BitcoinWallets.BitcoinWallet(
                new BitcoinWalletId(wallet.BitcoinWalletId),
                new PortfolioId(wallet.PortfolioId),
                new BitcoinAddress(wallet.Address),
                wallet.WalletName,
                new BlockchainNetwork(wallet.Network),
                wallet.ConnectedDate,
                new BitcoinAmount(wallet.Balance),
                wallet.LastSynced
            ));

        config
            .NewConfig<IBitcoinWallet, BitcoinWallet>()
            .Map(dest => dest.BitcoinWalletId, src => src.Id.Value)
            .Map(dest => dest.PortfolioId, src => src.PortfolioId.Value)
            .Map(dest => dest.Address, src => src.Address.Value)
            .Map(dest => dest.WalletName, src => src.WalletName)
            .Map(dest => dest.Network, src => src.Network.ChainId)
            .Map(dest => dest.ConnectedDate, src => src.ConnectedDate)
            .Map(dest => dest.LastSynced, src => src.LastSynced)
            .Map(dest => dest.Balance, src => src.Balance.Amount);
    }
}