namespace Hodler.Domain.Shared.Events;

public class DomainEventQueue
{
    private readonly Queue<IDomainEvent> _domainEventQueue = new();

    public void EnqueueDomainEvent(IDomainEvent domainEvent) => _domainEventQueue.Enqueue(domainEvent);

    public bool TryDequeueDomainEvent(out IDomainEvent? domainEvent) => _domainEventQueue.TryDequeue(out domainEvent);
}