namespace Hodler.Domain.Shared.Events;

public interface IDomainEvent
{
    DateTimeOffset OccuredOn { get; }
}