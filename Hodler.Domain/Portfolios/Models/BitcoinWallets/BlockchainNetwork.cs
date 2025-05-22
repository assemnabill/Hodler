namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public class BlockchainNetwork
{
    private const int BitcoinMainnetChainId = 1;

    public int ChainId { get; }
    public string Name { get; }

    public BlockchainNetwork(int chainId)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(chainId, nameof(chainId));

        ChainId = chainId;
        Name = chainId switch
        {
            BitcoinMainnetChainId => "Bitcoin Mainnet",
            _ => throw new ArgumentException("Unsupported blockchain network", nameof(chainId))
        };
    }
}