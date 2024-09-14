using Corz.DomainDriven.Abstractions.Failures;

namespace Hodler.Integration.ExternalApis.Failures;

internal class NoApiKeyProvidedFailure : Failure
{
    public string ServiceName { get; }

    public NoApiKeyProvidedFailure(string serviceName)
    {
        ServiceName = serviceName;
    }
}