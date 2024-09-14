using Hodler.Domain.User.Models;
using Mapster;

namespace Hodler.Integration.Repositories.User.Mappings;

public class UserMappingRegistration : IRegister
{
    // TODO
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UserSettings, Entities.UserSettings>()
            .Map(dest => dest.UserSettingsId, src => src.UserSettingsId)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Language, src => src.Language)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.Theme, src => src.Theme)
            .Map(dest => dest.Region, src => src.Region);

        config.ForType<Entities.UserSettings, UserSettings>()
            .MapWith(x => new UserSettings(
                x.UserSettingsId,
                new UserId(x.UserId),
                x.Language,
                x.Currency,
                x.Theme,
                x.Region));

        config.ForType<Entities.User, IUser>()
            .MapWith(x => new Domain.User.Models.User(
                new UserId(Guid.Parse(x.Id)),
                x.UserSettings != null
                    ? new UserSettings(
                        x.UserSettings.UserSettingsId,
                        new UserId(x.UserSettings.UserId),
                        x.UserSettings.Language,
                        x.UserSettings.Currency,
                        x.UserSettings.Theme,
                        x.UserSettings.Region)
                    : null,
                x.ApiKeys
                    .Select(y => new ApiKey(y.ApiName, y.Key))
                    .ToList())
            );

        config
            .NewConfig<Entities.ApiKey, ApiKey>()
            .MapWith(x => new ApiKey(x.ApiName, x.Key));

        config
            .NewConfig<ApiKey, Entities.ApiKey>()
            .MapWith(x => new Entities.ApiKey
            {
                ApiName = x.ApiName,
                Key = x.Key
            });
    }
}