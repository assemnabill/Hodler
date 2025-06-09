namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public class BlockchainNetwork
{
    private const int BitcoinMainnetChainId = 1;

    public int ChainId { get; }
    public string Name { get; }
    public static BlockchainNetwork BitcoinMainnet { get; } = new(BitcoinMainnetChainId);

    private BlockchainNetwork(int chainId)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(chainId, nameof(chainId));

        ChainId = chainId;
        Name = chainId switch
        {
            BitcoinMainnetChainId => "Bitcoin Mainnet",
            _ => throw new ArgumentException("Unsupported blockchain network", nameof(chainId))
        };
    }

    public static BlockchainNetwork FromChainId(int chainId)
    {
        return chainId switch
        {
            BitcoinMainnetChainId => BitcoinMainnet,
            _ => throw new ArgumentException("Unsupported blockchain network", nameof(chainId))
        };
    }
}