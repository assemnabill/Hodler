using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public class BitcoinWallet : IBitcoinWallet
{
    public BitcoinWallet(
        BitcoinWalletId id,
        PortfolioId portfolioId,
        BitcoinAddress address,
        string walletName,
        BlockchainNetwork network,
        DateTimeOffset connectedDate,
        BitcoinAmount balance,
        DateTimeOffset? lastSynced = null
    )
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(portfolioId);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(walletName);
        ArgumentNullException.ThrowIfNull(network);
        ArgumentNullException.ThrowIfNull(balance);

        Id = id;
        PortfolioId = portfolioId;
        Address = address;
        WalletName = walletName;
        Network = network;
        ConnectedDate = connectedDate;
        Balance = balance;
        LastSynced = lastSynced;
    }

    public BitcoinWalletId Id { get; }
    public PortfolioId PortfolioId { get; }
    public BitcoinAddress Address { get; }
    public string WalletName { get; }
    public BlockchainNetwork Network { get; }
    public DateTimeOffset ConnectedDate { get; }
    public DateTimeOffset? LastSynced { get; }
    public BitcoinAmount Balance { get; }

    public async Task<IBitcoinWallet> UpdateBalanceAsync(
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(blockchainService);

        var balance = await blockchainService
            .GetCurrentBalanceAsync(Address, cancellationToken);

        return balance == Balance
            ? this
            : new BitcoinWallet(
                id: Id,
                portfolioId: PortfolioId,
                address: Address,
                walletName: WalletName,
                network: Network,
                connectedDate: ConnectedDate,
                balance: balance,
                lastSynced: DateTimeOffset.UtcNow
            );
    }

    public static BitcoinWallet Create(
        PortfolioId portfolioId,
        BitcoinAddress address,
        string walletName,
        BlockchainNetwork network
    )
    {
        return new BitcoinWallet(
            id: new BitcoinWalletId(Guid.NewGuid()),
            portfolioId: portfolioId,
            address: address,
            walletName: walletName,
            network: network,
            connectedDate: DateTimeOffset.UtcNow,
            balance: BitcoinAmount.Zero
        );
    }
}