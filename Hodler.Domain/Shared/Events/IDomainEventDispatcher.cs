namespace Hodler.Domain.Shared.Events;

public interface IDomainEventDispatcher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

    Task PublishEventsOfAsync(DomainEventQueue queue, CancellationToken cancellationToken = default);
}