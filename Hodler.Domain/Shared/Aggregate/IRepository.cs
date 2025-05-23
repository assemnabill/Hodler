namespace Hodler.Domain.Shared.Aggregate;

public interface IRepository<in TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TAggregateRoot>
{
    Task StoreAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
}