using System.Collections.ObjectModel;
using Hodler.Domain.Portfolios.Services;
using ZLinq;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public class BitcoinWallets : ReadOnlyCollection<IBitcoinWallet>, IBitcoinWallets
{
    public BitcoinWallets(IEnumerable<IBitcoinWallet> list) : base(EnsureIsValid(list))
    {
    }

    public bool AlreadyConnected(BitcoinAddress address) =>
        this
            .AsValueEnumerable()
            .Any(w => w.Address.Value == address.Value);

    public bool AlreadyConnected(BitcoinWalletId walletId) =>
        this.AsValueEnumerable()
            .Any(w => w.Id.Value == walletId.Value);

    public async Task<IBitcoinWallets> ConnectWalletAsync(
        PortfolioId id,
        BitcoinAddress address,
        string walletName,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentException.ThrowIfNullOrWhiteSpace(walletName);

        var newWallet = BitcoinWallet.Create(id, address, walletName);

        // TODO: Do syncing async with events after implementing transactional outbox
        newWallet = await newWallet.SyncAsync(blockchainService, cancellationToken);

        return new BitcoinWallets(this.Append(newWallet).ToList());
    }

    public IBitcoinWallets Disconnect(BitcoinWalletId walletId) =>
        new BitcoinWallets(
            this
                .AsValueEnumerable()
                .Where(x => x.Id != walletId)
                .ToList()
        );

    public async Task<IBitcoinWallets> SyncWalletAsync(
        BitcoinWalletId walletId,
        IBitcoinBlockchainService blockchainService,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(walletId);
        ArgumentNullException.ThrowIfNull(blockchainService);

        var wallet = FindById(walletId);

        if (wallet == null)
            return this;

        try
        {
            var updatedWallet = await wallet.SyncAsync(blockchainService, cancellationToken);

            return new BitcoinWallets(this
                .AsValueEnumerable()
                .Where(x => x.Id.Value != walletId.Value)
                .Append(updatedWallet)
                .ToList()
            );
        }
        catch (Exception ex)
        {
            return this;
        }

    }

    public IBitcoinWallet? FindById(BitcoinWalletId walletId) =>
        this
            .AsValueEnumerable()
            .FirstOrDefault(w => w.Id == walletId);

    private static IList<IBitcoinWallet> EnsureIsValid(IEnumerable<IBitcoinWallet> list)
    {
        return list
            .AsValueEnumerable()
            .OrderByDescending(x => x.ConnectedDate)
            .ToList();
    }
}