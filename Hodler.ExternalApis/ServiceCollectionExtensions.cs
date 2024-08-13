using Hodler.Domain.Transactions.Ports.DiaDataApi;

namespace Hodler.ExternalApis;

public class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        services.AddTransient<IDiaDataApiClient, DiaDataApiClient>();

        return services;
    }
}

