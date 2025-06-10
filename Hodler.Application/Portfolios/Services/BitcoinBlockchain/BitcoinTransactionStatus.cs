using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BitcoinTransactionStatus
{
    [JsonPropertyName("confirmed")]
    public bool Confirmed { get; set; }

    [JsonPropertyName("block_height")]
    public long BlockHeight { get; set; }

    [JsonPropertyName("block_hash")]
    public string BlockHash { get; set; }

    [JsonPropertyName("block_time")]
    public long BlockTime { get; set; }
}