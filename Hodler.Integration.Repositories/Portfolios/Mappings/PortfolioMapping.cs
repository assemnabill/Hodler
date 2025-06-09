using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Hodler.Integration.Repositories.Portfolios.Entities;
using Mapster;
using BitcoinWallet = Hodler.Integration.Repositories.Portfolios.Entities.BitcoinWallet;
using Portfolio = Hodler.Integration.Repositories.Portfolios.Entities.Portfolio;
using BlockchainTransaction = Hodler.Integration.Repositories.Portfolios.Entities.BlockchainTransaction;

namespace Hodler.Integration.Repositories.Portfolios.Mappings;

public class PortfolioMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Portfolio, IPortfolio>()
            .MapWith(portfolio => new Domain.Portfolios.Models.Portfolio(
                new PortfolioId(portfolio.PortfolioId),
                new ManualTransactions(portfolio.ManualTransactions.Select(x =>
                    x.Adapt<ManualTransaction, Transaction>())
                ),
                new UserId(Guid.Parse(portfolio.UserId)),
                new BitcoinWallets(portfolio.BitcoinWallets
                    .Select(x => x.Adapt<BitcoinWallet, IBitcoinWallet>())
                    .ToList()
                )
            ));

        config
            .NewConfig<IPortfolio, Portfolio>()
            .MapWith(x => new Portfolio
            {
                PortfolioId = x.Id.Value,
                UserId = x.UserId.Value.ToString(),
                ManualTransactions = x.ManualTransactions
                    .Select(t => t.Adapt<Transaction, ManualTransaction>())
                    .ToList(),
                BitcoinWallets = x.BitcoinWallets
                    .Select(t => t.Adapt<IBitcoinWallet, BitcoinWallet>())
                    .ToList()
            });

        config
            .NewConfig<ManualTransaction, Transaction>()
            .MapWith(transaction => new Transaction(
                new PortfolioId(transaction.PortfolioId),
                new TransactionId(transaction.TransactionId),
                (TransactionType)transaction.Type,
                new FiatAmount(transaction.FiatAmount, FiatCurrency.GetById(transaction.FiatCurrency)),
                new BitcoinAmount(transaction.BtcAmount),
                transaction.Timestamp.ToUniversalTime(),
                new FiatAmount(transaction.MarketPrice, FiatCurrency.GetById(transaction.FiatCurrency)),
                transaction.SourceType == null
                    ? null
                    : transaction.SourceType == (int)TransactionSourceType.Wallet
                        ? TransactionSource.FromWallet(
                            transaction.SourceIdentifier == null ? null : new BitcoinWalletId(Guid.Parse(transaction.SourceIdentifier)),
                            transaction.SourceName
                        )
                        : TransactionSource.FromExchange(
                            transaction.SourceIdentifier == null ? null : (CryptoExchangeName)int.Parse(transaction.SourceIdentifier),
                            transaction.SourceName
                        ),
                transaction.Fee == null ? null : new BitcoinAmount(transaction.Fee.Value))
            );

        config
            .NewConfig<Transaction, ManualTransaction>()
            .MapWith(x => new ManualTransaction
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
                BlockchainNetwork.FromChainId(wallet.Network),
                wallet.ConnectedDate,
                new BitcoinAmount(wallet.Balance),
                wallet.BlockchainTransactions
                    .Select(t => t.Adapt<BlockchainTransaction, Domain.Portfolios.Models.BitcoinWallets.BlockchainTransaction>())
                    .ToList(),
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
            .Map(dest => dest.BlockchainTransactions, src => src.Transactions)
            .Map(dest => dest.Balance, src => src.Balance.Amount);

        config
            .NewConfig<BlockchainTransaction, Domain.Portfolios.Models.BitcoinWallets.BlockchainTransaction>()
            .MapWith(x => new Domain.Portfolios.Models.BitcoinWallets.BlockchainTransaction(
                new BitcoinAmount(x.BtcAmount),
                new TransactionHash(x.TransactionHash),
                new FiatAmount(x.MarketPriceInUsd, FiatCurrency.UsDollar),
                new FiatAmount(x.FiatValueInUsd, FiatCurrency.UsDollar),
                x.Timestamp,
                x.Status.ToEnum<BlockchainTransactionStatus>(),
                new BitcoinAddress(x.FromAddress),
                new BitcoinAddress(x.ToAddress),
                new BitcoinAmount(x.NetworkFeeInBtc),
                new FiatAmount(x.NetworkFeeInUsd, FiatCurrency.UsDollar),
                x.Note,
                x.Type.ToEnum<TransactionType>(),
                new PortfolioId(x.PortfolioId),
                new BitcoinWalletId(x.BitcoinWalletId)
            ));

        config
            .NewConfig<Domain.Portfolios.Models.BitcoinWallets.BlockchainTransaction, BlockchainTransaction>()
            .Map(dest => dest.PortfolioId, src => src.PortfolioId.Value)
            .Map(dest => dest.BitcoinWalletId, src => src.BitcoinWalletId.Value)
            .Map(dest => dest.BtcAmount, src => src.Amount.Amount)
            .Map(dest => dest.TransactionHash, src => src.TransactionHash.Value)
            .Map(dest => dest.MarketPriceInUsd, src => src.MarketPrice.Amount)
            .Map(dest => dest.FiatValueInUsd, src => src.FiatValue.Amount)
            .Map(dest => dest.Timestamp, src => src.Timestamp)
            .Map(dest => dest.Status, src => (int)src.Status)
            .Map(dest => dest.FromAddress, src => src.FromAddress.Value)
            .Map(dest => dest.ToAddress, src => src.ToAddress.Value)
            .Map(dest => dest.NetworkFeeInBtc, src => src.NetworkFee.Amount)
            .Map(dest => dest.NetworkFeeInUsd, src => src.FiatFee.Amount)
            .Map(dest => dest.Note, src => src.Note)
            .Map(dest => dest.Type, src => (int)src.TransactionType);
    }
}