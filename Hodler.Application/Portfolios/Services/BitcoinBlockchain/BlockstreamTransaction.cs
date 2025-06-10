using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public record BlockstreamTransaction(
    string TxId,
    BlockstreamTxStatus Status,
    long Fee,
    BlockstreamInput[] Inputs,
    BlockstreamOutput[] Outputs
);

public record BlockstreamTxStatus(bool Confirmed, long BlockTime);

public record BlockstreamInput(BlockstreamPrevOut PrevOut);

public record BlockstreamPrevOut(string ScriptPubKeyAddress);

public record BlockstreamOutput(string ScriptPubKeyAddress, long Value);

public record BlockstreamAddressInfo(BlockstreamChainStats ChainStats);

public record BlockstreamChainStats(long FundedTxoSum);

public record BlockchainInfoTicker(BlockchainInfoCurrency USD);

public record BlockchainInfoCurrency(decimal Last);

public class BlockstreamAddressReponse
{
    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("chain_stats")]
    public Stats ChainStats { get; set; }

    [JsonPropertyName("mempool_stats")]
    public Stats MempoolStats { get; set; }
}

public class Stats
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