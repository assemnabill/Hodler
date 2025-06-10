using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BitcoinTransactionInputs
{
    [JsonPropertyName("txid")]
    public string Txid { get; set; }

    [JsonPropertyName("vout")]
    public long Vout { get; set; }

    [JsonPropertyName("prevout")]
    public BitcoinTransactionOutput Prevout { get; set; }

    [JsonPropertyName("scriptsig")]
    public string Scriptsig { get; set; }

    [JsonPropertyName("scriptsig_asm")]
    public string ScriptsigAsm { get; set; }

    [JsonPropertyName("witness")]
    public List<string> Witness { get; set; }

    [JsonPropertyName("is_coinbase")]
    public bool IsCoinbase { get; set; }

    [JsonPropertyName("sequence")]
    public long Sequence { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("inner_redeemscript_asm")]
    public string InnerRedeemscriptAsm { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("inner_witnessscript_asm")]
    public string InnerWitnessscriptAsm { get; set; }
}