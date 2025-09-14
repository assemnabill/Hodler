using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Mapster;
using Mapster.Utils;
using User = Hodler.Integration.Repositories.Users.Entities.User;

namespace Hodler.Integration.Repositories.Users.Mappings;

public class UserMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<UserSettings, Entities.UserSettings>()
            .Map(dest => dest.UserSettingsId, src => src.UserSettingsId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Currency, src => src.DisplayCurrency.Id)
            .Map(dest => dest.Theme, src => src.Theme);

        config
            .ForType<Entities.UserSettings, UserSettings>()
            .MapWith(x => new UserSettings(
                x.UserSettingsId,
                new UserId(Guid.Parse(x.UserId)),
                FiatCurrency.GetById(x.Currency),
                x.Theme.ToEnum<AppTheme>()
            ));

        config
            .ForType<User, IUser>()
            .MapWith(x =>
                new Domain.Users.Models.User(
                    new UserId(Guid.Parse(x.Id)),
                    new ContactInfo
                    (
                        new Domain.Users.ValueObjects.UserName(x.UserName),
                        new Domain.Users.ValueObjects.PhoneNumber(x.PhoneNumber),
                        new Domain.Users.ValueObjects.EmailAddress(x.Email)
                    ),
                    x.UserSettings == null
                        ? null
                        : new UserSettings(
                            x.UserSettings.UserSettingsId,
                            new UserId(Guid.Parse(x.UserSettings.UserId)),
                            FiatCurrency.GetById(x.UserSettings.Currency),
                            x.UserSettings.Theme.ToEnum<AppTheme>()
                        )
                    ,
                    x.ApiKeys
                        .Select(y => y.Adapt<ApiKey>())
                        .ToList()
                )
            );

        config
            .ForType<Entities.ApiKey, ApiKey>()
            .MapWith(x => new ApiKey(
                    new ApiKeyId(x.ApiKeyId),
                    Enum<ApiKeyName>.Parse(x.ApiKeyName),
                    x.Value,
                    new UserId(Guid.Parse(x.UserId)),
                    x.Secret
                )
            );

        config
            .ForType<ApiKey, Entities.ApiKey>()
            .MapWith(x => new Entities.ApiKey
            {
                ApiKeyId = x.ApiKeyId.Value,
                UserId = x.UserId.ToString(),
                ApiKeyName = x.ApiKeyName.GetDescription(),
                Value = x.Value,
                Secret = x.Secret
            });

        config
         .ForType<IUser, User>()
         .Map(dest => dest.UserName, src => src.ContactInfo.UserName.Value)
         .Map(dest => dest.PhoneNumber, src => src.ContactInfo.PhoneNumber.Value)
         .Map(dest => dest.Email, src => src.ContactInfo.Email.Value);

    }
}