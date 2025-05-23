using Hodler.Contracts.Portfolios.AddTransaction;
using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Shared;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Models.Transactions;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class PortfolioInfoMapping : IRegister
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
                    src.Timestamp,
                    src.TransactionSource == null ? null : src.TransactionSource.Adapt<TransactionSourceDto>()
                ));

        config
            .NewConfig<ITransactionSource, TransactionSourceDto>()
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Identifier, src => src.Identifier)
            .Map(dest => dest.Name, src => src.Name);

        config
            .NewConfig<TransactionSourceDto, ITransactionSource>()
            .MapWith(src =>
                src.Type == (int)TransactionSourceType.Wallet
                    ? TransactionSource.FromWallet(new BitcoinWalletId(Guid.Parse(src.Identifier)), src.Name)
                    : TransactionSource.FromExchange((CryptoExchangeName)int.Parse(src.Identifier), src.Name)
            );
    }
}