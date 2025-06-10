using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BlockstreamAddressResponse
{
    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("chain_stats")]
    public BlockstreamBitcoinStatistics ChainStatistics { get; set; }

    [JsonPropertyName("mempool_stats")]
    public BlockstreamBitcoinStatistics MempoolStatistics { get; set; }
}