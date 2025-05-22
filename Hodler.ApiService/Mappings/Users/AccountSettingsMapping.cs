using Corz.Extensions.Enumeration;
using Hodler.Contracts.Users;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Mapster;

namespace Hodler.ApiService.Mappings.Users;

public class AccountSettingsMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<UserSettings, AccountSettingsContract>()
            .Map(dest => dest.Theme, src => src.Theme.GetDescription())
            .Map(dest => dest.DisplayCurrency, src => src.DisplayCurrency);

        config
            .NewConfig<FiatCurrency, FiatCurrencyContract>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Ticker, src => src.Ticker)
            .Map(dest => dest.Symbol, src => src.Symbol);
    }
}