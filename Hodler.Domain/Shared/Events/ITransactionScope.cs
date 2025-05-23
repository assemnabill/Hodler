namespace Hodler.Domain.Shared.Events;

[Obsolete]
public interface ITransactionScope : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}