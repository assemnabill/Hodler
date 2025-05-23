namespace Hodler.Domain.Shared.Exceptions;

public class AggregateDomainException : Exception
{
    public IReadOnlyCollection<DomainException> InnerExceptions { get; }

    public AggregateDomainException(IEnumerable<DomainException> domainExceptions)
    {
        InnerExceptions = domainExceptions.ToList();
    }
}