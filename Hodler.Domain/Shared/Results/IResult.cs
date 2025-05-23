using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Shared.Results;

/// <summary>
///     Defines the result of an operation indicating success or failure.
/// </summary>
public interface IResult
{
    FailureCollection Failures { get; }

    bool IsSuccess { get; }
}

/// <summary>
///     Defines the result of an operation coantaining a value of Type <see cref="TValue" /> indicating success or failure.
/// </summary>
/// <typeparam name="TValue">The type of the value contained by the instance of this interface.</typeparam>
public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}