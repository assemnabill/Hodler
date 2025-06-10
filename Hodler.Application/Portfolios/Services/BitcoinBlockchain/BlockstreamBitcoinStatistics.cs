using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BlockstreamBitcoinStatistics
{
    [JsonPropertyName("funded_txo_count")]
    public long FundedTxoCount { get; set; }

    [JsonPropertyName("funded_txo_sum")]
    public long FundedTxoSum { get; set; }

    [JsonPropertyName("spent_txo_count")]
    public long SpentTxoCount { get; set; }

    [JsonPropertyName("spent_txo_sum")]
    public long SpentTxoSum { get; set; }

    [JsonPropertyName("tx_count")]
    public long TxCount { get; set; }
}