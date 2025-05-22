namespace Hodler.Contracts.WalletConnections;

public record ConnectedWalletDto(
    Guid Id,
    string Name,
    string Address,
    string Network,
    DateTimeOffset ConnectedDate,
    DateTimeOffset? LastSynced
);