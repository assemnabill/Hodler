using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;
using Mapster;
using Mapster.Utils;

namespace Hodler.Integration.Repositories.User.Mappings;

public class UserMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<UserSettings, Entities.UserSettings>()
            .Map(dest => dest.UserSettingsId, src => src.UserSettingsId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Language, src => src.Language)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.Theme, src => src.Theme)
            .Map(dest => dest.Region, src => src.Region);

        config
            .ForType<Entities.UserSettings, UserSettings>()
            .MapWith(x => new UserSettings(
                x.UserSettingsId,
                new UserId(Guid.Parse(x.UserId)),
                x.Language,
                x.Currency,
                x.Theme,
                x.Region));

        config
            .ForType<Entities.User, IUser>()
            .MapWith(x =>
                new Domain.User.Models.User(
                    new UserId(Guid.Parse(x.Id)),
                    x.UserSettings != null
                        ? new UserSettings(
                            x.UserSettings.UserSettingsId,
                            new UserId(Guid.Parse(x.UserSettings.UserId)),
                            x.UserSettings.Language,
                            x.UserSettings.Currency,
                            x.UserSettings.Theme,
                            x.UserSettings.Region)
                        : null,
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
    }
}