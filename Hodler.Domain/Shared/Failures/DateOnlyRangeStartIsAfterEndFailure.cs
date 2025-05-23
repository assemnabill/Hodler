namespace Hodler.Domain.Shared.Failures;

public class DateOnlyRangeStartIsAfterEndFailure : Failure
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public DateOnlyRangeStartIsAfterEndFailure(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }
}