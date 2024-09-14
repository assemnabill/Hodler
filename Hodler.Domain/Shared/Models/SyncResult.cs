namespace Hodler.Domain.Shared.Models;

public class SyncResult<T> where T : class
{
    public bool Changed { get; }
    public T CurrentState { get; }

    public SyncResult(bool changed, T currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState);

        Changed = changed;
        CurrentState = currentState;
    }
}