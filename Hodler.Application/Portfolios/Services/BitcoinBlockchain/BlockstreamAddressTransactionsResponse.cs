using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BlockstreamAddressTransactionsResponse
{
    [JsonPropertyName("txid")]
    public string Txid { get; set; }

    [JsonPropertyName("version")]
    public long Version { get; set; }

    [JsonPropertyName("locktime")]
    public long Locktime { get; set; }

    [JsonPropertyName("vin")]
    public List<BitcoinTransactionInputs> Vin { get; set; }

    [JsonPropertyName("vout")]
    public List<BitcoinTransactionOutput> Vout { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("weight")]
    public long Weight { get; set; }

    [JsonPropertyName("fee")]
    public long Fee { get; set; }

    [JsonPropertyName("status")]
    public BitcoinTransactionStatus BitcoinTransactionStatus { get; set; }
}