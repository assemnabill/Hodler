namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public enum ScriptPublicKeyType
{
    P2pkh, // Pay to Public Key Hash
    P2sh, // Pay to Script Hash
    P2wpkh, // Pay to Witness Public Key Hash
    P2wsh, // Pay to Witness Script Hash
    P2tr, // Pay to Taproot
    Unknown // For any unrecognized values
};