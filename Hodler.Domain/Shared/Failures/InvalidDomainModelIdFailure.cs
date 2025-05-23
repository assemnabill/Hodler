namespace Hodler.Domain.Shared.Failures;

public class InvalidDomainModelIdFailure : Failure
{
    public Type Type { get; }
    public Guid Id { get; }

    public InvalidDomainModelIdFailure(Type type, Guid id)
    {
        Type = type;
        Id = id;
    }
}