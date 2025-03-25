using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBitcoinPrice;

public class CoinCapCandlesResponse
{
    [JsonPropertyName("data")] public Candle[] Candles { get; set; }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }
}