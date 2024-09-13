using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Integration.Repositories.Portfolio.Repositories;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPortfolioRepository, PortfolioRepository>();

        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}