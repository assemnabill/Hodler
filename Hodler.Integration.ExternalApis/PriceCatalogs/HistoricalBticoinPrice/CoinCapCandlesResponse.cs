using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBticoinPrice;

public class CoinCapCandlesResponse
{
    [JsonPropertyName("data")] 
    public Candle[] Candles { get; set; }

    [JsonPropertyName("timestamp")] 
    public long Timestamp { get; set; }
}