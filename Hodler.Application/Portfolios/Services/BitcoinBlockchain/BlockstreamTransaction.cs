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