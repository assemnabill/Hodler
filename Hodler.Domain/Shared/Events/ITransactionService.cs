namespace Hodler.Domain.Shared.Events;

[Obsolete]
public interface ITransactionService
{
    Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken);

    Task<ITransactionScope> BeginTransactionAsync(
        TransactionIsolationLevel transactionIsolationLevel,
        CancellationToken cancellationToken
    );
}