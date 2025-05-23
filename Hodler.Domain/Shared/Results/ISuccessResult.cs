namespace Hodler.Domain.Shared.Results;

/// <summary>
///     Defines the success of an operation.
/// </summary>
public interface ISuccessResult : IResult;

/// <summary>
///     Defines the success of an operation coantaining a value of Type <see cref="TValue" />.
/// </summary>
/// <typeparam name="TValue">The type of the value contained by the instance of this interface.</typeparam>
public interface ISuccessResult<out TValue> : ISuccessResult, IResult<TValue>;