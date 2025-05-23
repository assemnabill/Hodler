using System.Reflection;
using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Shared.Exceptions;

public abstract class DomainException : Exception
{
    public Failure Failure { get; }

    protected DomainException(Failure failure)
        : base(failure.Message)
    {
        Failure = failure;
    }

    protected DomainException(Failure failure, Exception innerException)
        : base(failure.Message, innerException)
    {
        Failure = failure;
    }

    public static DomainException CreateFrom(Failure failure)
    {
        failure.EnsureMessageIsDefined();
        var failureType = failure.GetType();
        var genericDomainExceptionType = typeof(DomainException<>);
        var specificDomainExceptionType = genericDomainExceptionType.MakeGenericType(failureType);

        var method = specificDomainExceptionType.GetMethod(
            "CreateFrom",
            BindingFlags.Static | BindingFlags.NonPublic,
            null,
            [failureType],
            null
        );
        var result = method?.Invoke(null, [failure]);
        return result as DomainException ?? throw new InvalidOperationException();
    }

    public static DomainException CreateFrom(Failure failure, Exception innerException)
    {
        failure.EnsureMessageIsDefined();
        var failureType = failure.GetType();
        var genericDomainExceptionType = typeof(DomainException<>);
        var specificDomainExceptionType = genericDomainExceptionType.MakeGenericType(failureType);

        var method = specificDomainExceptionType.GetMethod(
            "CreateFrom",
            BindingFlags.Static | BindingFlags.NonPublic,
            null,
            [failureType, typeof(Exception)],
            null
        );
        var result = method?.Invoke(
            null,
            [failure, innerException]
        );
        return result as DomainException ?? throw new InvalidOperationException();
    }
}

public class DomainException<T> : DomainException where T : Failure
{
    private DomainException(T failure)
        : base(failure)
    {
    }

    private DomainException(T failure, Exception innerException)
        : base(failure, innerException)
    {
    }

    // Used via reflection
    // ReSharper disable once UnusedMember.Local
    private static DomainException CreateFrom(T failure) => new DomainException<T>(failure);

    // Used via reflection
    // ReSharper disable once UnusedMember.Local
    private static DomainException CreateFrom(T failure, Exception innerException) => new DomainException<T>(failure, innerException);
}