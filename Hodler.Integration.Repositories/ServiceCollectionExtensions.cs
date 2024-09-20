using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.User.Ports;
using Hodler.Integration.Repositories.Portfolio.Repositories;
using Hodler.Integration.Repositories.User.Repositories;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}