using Hodler.Contracts.Portfolios.Wallets;
using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class BitcoinWalletMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<IBitcoinWallet, BitcoinWalletDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.WalletName, src => src.WalletName)
            .Map(dest => dest.Address, src => src.Address.Value)
            .Map(dest => dest.Balance, src => src.Balance.Amount)
            .Map(dest => dest.ConnectedDate, src => src.ConnectedDate)
            .Map(dest => dest.LastSynced, src => src.LastSynced)
            .Map(dest => dest.Transactions, src => src.Transactions)
            .Map(dest => dest.Icon, src => src.Avatar.Icon.Value)
            .Map(dest => dest.Color, src => src.Avatar.Color.Value);

        config
            .NewConfig<IReadOnlyCollection<IBitcoinWallet>, ConnectedWalletsDto>()
            .Map(dest => dest.Wallets,
                src => src
                    .Select(x => x.Adapt<BitcoinWalletDto>())
                    .ToList()
            );

        config
            .NewConfig<BlockchainTransaction, BlockchainTransactionDto>()
            .MapWith(x => new BlockchainTransactionDto(
                x.NetBitcoin.Amount,
                x.TransactionHash.Value,
                x.MarketPrice.Adapt<FiatAmountDto>(),
                x.FiatValue.Adapt<FiatAmountDto>(),
                x.Timestamp,
                x.Status,
                x.FromAddress.Value,
                x.ToAddress.Value,
                x.NetworkFee.Amount,
                x.FiatFee.Adapt<FiatAmountDto>(),
                x.Note,
                x.TransactionType,
                x.BitcoinWalletId.Value
            ));
    }
}