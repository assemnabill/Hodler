namespace Hodler.Domain.Shared.Results;

/// <summary>
///     Defines the failure of an operation containing a failure code.
/// </summary>
public interface IFailureResult : IResult;

/// <summary>
///     Defines the failure of an operation containing a failure code and a value of type <see cref="TValue" />.
/// </summary>
/// <typeparam name="TValue">The type of the value contained by the instance of this interface.</typeparam>
public interface IFailureResult<out TValue> : IFailureResult, IResult<TValue>;