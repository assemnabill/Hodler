using System.ComponentModel;

namespace Hodler.Domain.CryptoExchange.Models;

public enum ApiKeyName
{
    [Description("BitPanda - ApiKey")] BitPandaApiKey = 1,
    [Description("Kraken - ApiKey")] KrakenApiKey = 2,
    [Description("Kraken - ApiSign")] KrakenApiSign = 3
}