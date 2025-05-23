using System.Collections;
using Hodler.Domain.Shared.Exceptions;

namespace Hodler.Domain.Shared.Failures;

public class FailureCollection : IEnumerable<Failure>
{
    private readonly List<Failure> _failures;

    public static FailureCollection Empty => [];

    public FailureCollection()
    {
        _failures = [];
    }

    public FailureCollection(IEnumerable<Failure> failures)
    {
        _failures = failures?.ToList() ?? [];
    }

    public FailureCollection(params Failure[] failures)
    {
        _failures = failures?.ToList() ?? [];
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_failures).GetEnumerator();

    public IEnumerator<Failure> GetEnumerator() => _failures.GetEnumerator();

    public void Add(Failure failure)
    {
        _failures.Add(failure);
    }

    public IReadOnlyCollection<Failure> AsReadOnly() => _failures.AsReadOnly();

    public void ThrowIfAnyFailure()
    {
        if (!_failures.Any())
            return;

        if (_failures.Count == 1)
            throw DomainException.CreateFrom(_failures.Single());

        throw new AggregateDomainException(_failures.Select(DomainException.CreateFrom));
    }

    public override string ToString() => string.Join(" ", _failures);
}