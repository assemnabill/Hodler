using Hodler.Contracts.WalletConnections;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Mapster;

namespace Hodler.ApiService.Mappings.Portfolios;

public class BitcoinWalletMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IBitcoinWallet, ConnectedWalletDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.WalletName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Network, src => src.Network.Name)
            .Map(dest => dest.ConnectedDate, src => src.ConnectedDate)
            .Map(dest => dest.LastSynced, src => src.LastSynced);

        config.NewConfig<IReadOnlyCollection<IBitcoinWallet>, ConnectedWalletsDto>()
            .Map(dest => dest.Wallets,
                src => src
                    .Select(x => x.Adapt<ConnectedWalletDto>())
                    .ToList()
            );
    }
}