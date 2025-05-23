using Hodler.Domain.Shared.Failures;

namespace Hodler.Domain.Shared.Results;

public class FailureResult : IFailureResult
{
    public FailureResult(Failure failure)
    {
        Failures = [failure];
    }

    public FailureResult(FailureCollection failure)
    {
        Failures = failure;
    }

    public FailureCollection Failures { get; }
    public bool IsSuccess { get; } = false;

    public override string ToString() => Failures.ToString();
}

public class FailureResult<TValue> : FailureResult, IFailureResult<TValue>
{
    public FailureResult(Failure failure)
        : base(failure)
    {
    }

    public FailureResult(FailureCollection failure)
        : base(failure)
    {
    }

    public TValue Value { get; } = default;
}