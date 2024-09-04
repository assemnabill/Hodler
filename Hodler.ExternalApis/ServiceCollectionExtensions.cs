using Hodler.Domain.Transactions.Ports.DiaDataApi;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.ExternalApis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        services.AddTransient<IDiaDataApiClient, DiaDataApiClient>();

        return services;
    }
}

