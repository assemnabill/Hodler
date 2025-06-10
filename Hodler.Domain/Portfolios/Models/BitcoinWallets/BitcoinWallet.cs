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
        IReadOnlyCollection<BlockchainTransaction> transactions,
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
        Transactions = transactions;
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
    public IReadOnlyCollection<BlockchainTransaction> Transactions { get; }

    public async Task<IBitcoinWallet> SyncAsync(
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    )
    {
        var transactions = Transactions;
        var newBalance = await blockchainService.GetCurrentBalanceAsync(Address, cancellationToken);

        if (newBalance != BitcoinAmount.Zero && Balance != newBalance)
        {
            transactions = (await blockchainService.GetTransactionsAsync(this, cancellationToken))
                .Where(t => t.NetBitcoin != BitcoinAmount.Zero)
                .ToList();
        }

        return new BitcoinWallet(
            Id,
            PortfolioId,
            Address,
            WalletName,
            Network,
            ConnectedDate,
            newBalance,
            transactions,
            DateTimeOffset.UtcNow
        );
    }

    public static IBitcoinWallet Create(
        PortfolioId portfolioId,
        BitcoinAddress address,
        string walletName
    )
    {
        return new BitcoinWallet(
            id: new BitcoinWalletId(Guid.NewGuid()),
            portfolioId: portfolioId,
            address: address,
            walletName: walletName,
            network: BlockchainNetwork.BitcoinMainnet,
            connectedDate: DateTimeOffset.UtcNow,
            balance: BitcoinAmount.Zero,
            transactions: new List<BlockchainTransaction>()
        );
    }
}