namespace Hodler.Domain.Shared.Aggregate;

public interface IEntity<out TId> where TId : struct
{
    TId Id { get; }
}