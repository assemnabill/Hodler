using Hodler.Domain.Shared.Events;

namespace Hodler.Domain.Shared.Aggregate;

public abstract class AggregateRoot<TAggregateRoot> : IAggregateRoot<TAggregateRoot>
    where TAggregateRoot : AggregateRoot<TAggregateRoot>
{
    public DomainEventQueue DomainEventQueue { get; } = new();

    public virtual void OnBeforeStore()
    {
    }

    public virtual void OnAfterStore()
    {
    }

    protected void EnqueueDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        DomainEventQueue.EnqueueDomainEvent(domainEvent);
    }

    protected void EnqueueDomainEvents(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            EnqueueDomainEvent(domainEvent);
    }
}