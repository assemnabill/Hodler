using Hodler.Contracts.Portfolios.Wallets;
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
            .Map(dest => dest.LastSynced, src => src.LastSynced);

        config
            .NewConfig<IReadOnlyCollection<IBitcoinWallet>, ConnectedWalletsDto>()
            .Map(dest => dest.Wallets,
                src => src
                    .Select(x => x.Adapt<BitcoinWalletDto>())
                    .ToList()
            );
    }
}