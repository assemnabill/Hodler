namespace Hodler.Domain.Shared.Events;

public abstract class DomainEventBase : IDomainEvent
{
    protected DomainEventBase(DateTimeOffset? occuredOn = null)
    {
        OccuredOn = occuredOn ?? DateTimeOffset.UtcNow;
    }

    public DateTimeOffset OccuredOn { get; }
}