namespace Hodler.Domain.Shared.Exceptions;

public class MissingFailureMessageException : Exception
{
    public Type Type { get; }

    public MissingFailureMessageException(Type type)
        : base($"The message of the failure '{type.Name}' is missing.")
    {
        Type = type;
    }
}