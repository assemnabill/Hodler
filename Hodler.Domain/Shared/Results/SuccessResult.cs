using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Shared.Results;

public class SuccessResult : ISuccessResult
{
    public FailureCollection Failures => [];

    public bool IsSuccess { get; } = true;
}

public class SuccessResult<TValue> : SuccessResult, ISuccessResult<TValue>
{
    public SuccessResult(TValue value)
    {
        Value = value;
    }

    public TValue Value { get; }
}