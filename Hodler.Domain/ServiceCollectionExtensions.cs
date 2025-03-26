using Hodler.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
  
        services
            .AddTransient<IUserSettingsQueryService, UserSettingsQueryService>()
            .AddTransient<IUserSettingsService, UserSettingsService>();

        return services;
    }
}