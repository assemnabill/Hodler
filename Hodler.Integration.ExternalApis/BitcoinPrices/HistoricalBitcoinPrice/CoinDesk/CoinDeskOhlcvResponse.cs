using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

public class CoinDeskOhlcvResponse
{
    [JsonPropertyName("Data")]
    public CoinDeskCandle[] Data { get; set; }

    [JsonPropertyName("Err")]
    public Err Err { get; set; }
}