using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthCore(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        
        return services;
    }
}