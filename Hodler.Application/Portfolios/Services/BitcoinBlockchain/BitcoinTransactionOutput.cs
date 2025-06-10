using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class BitcoinTransactionOutput
{
    [JsonPropertyName("scriptpubkey")]
    public string Scriptpubkey { get; set; }

    [JsonPropertyName("scriptpubkey_asm")]
    public string ScriptpubkeyAsm { get; set; }

    [JsonPropertyName("scriptpubkey_type")]
    public ScriptPublicKeyType ScriptPublicKeyType { get; set; }

    [JsonPropertyName("scriptpubkey_address")]
    public string ScriptpubkeyAddress { get; set; }

    [JsonPropertyName("value")]
    public long Value { get; set; }
}