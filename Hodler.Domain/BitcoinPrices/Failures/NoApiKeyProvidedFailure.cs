using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.BitcoinPrices.Failures;

public class NoApiKeyProvidedFailure : Failure
{
    public string ServiceName { get; }

    public NoApiKeyProvidedFailure(string serviceName)
    {
        ServiceName = serviceName;
    }
}