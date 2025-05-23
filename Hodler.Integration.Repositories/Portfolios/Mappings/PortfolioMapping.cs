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
            .MapWith(x => new Portfolio
            {
                PortfolioId = x.Id.Value,
                UserId = x.UserId.Value.ToString(),
                Transactions = x.Transactions
                    .Select(t => t.Adapt<Domain.Portfolios.Models.Transactions.Transaction, Transaction>())
                    .ToList(),
                BitcoinWallets = x.BitcoinWallets
                    .Select(t => t.Adapt<IBitcoinWallet, BitcoinWallet>())
                    .ToList()
            });

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
                transaction.Fee == null ? null : new BitcoinAmount(transaction.Fee.Value))
            );

        config
            .NewConfig<Domain.Portfolios.Models.Transactions.Transaction, Transaction>()
            .MapWith(x => new Transaction
            {
                PortfolioId = x.PortfolioId.Value,
                TransactionId = x.Id.Value,
                Type = (int)x.Type,
                FiatAmount = x.FiatAmount.Amount,
                FiatCurrency = x.FiatAmount.FiatCurrency.Id,
                BtcAmount = x.BtcAmount.Amount,
                MarketPrice = x.MarketPrice.Amount,
                Timestamp = x.Timestamp.UtcDateTime,
                Fee = x.TransactionFee == null ? null : x.TransactionFee.Amount,
                SourceType = x.TransactionSource == null ? null : (int)x.TransactionSource.Type,
                SourceIdentifier = x.TransactionSource == null ? null : x.TransactionSource.Identifier,
                SourceName = x.TransactionSource == null ? null : x.TransactionSource.Name
            });

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