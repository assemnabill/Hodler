namespace Hodler.Domain.Shared.Events;

public enum TransactionIsolationLevel
{
    ReadUncommitted,
    ReadCommitted,
    RepeatableRead,
    Serializable,
    Snapshot
}