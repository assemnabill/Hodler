using Hodler.Domain.Shared.Events;

namespace Hodler.Domain.Shared.Aggregate;

public interface IAggregateRoot<TAggregateRoot> where TAggregateRoot : IAggregateRoot<TAggregateRoot>
{
    DomainEventQueue DomainEventQueue { get; }

    public void OnBeforeStore();

    public void OnAfterStore();
}