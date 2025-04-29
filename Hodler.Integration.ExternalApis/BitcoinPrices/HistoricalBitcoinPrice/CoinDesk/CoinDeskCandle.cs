using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

public class CoinDeskCandle
{
    [JsonPropertyName("UNIT")]
    public string Unit { get; set; }

    [JsonPropertyName("TIMESTAMP")]
    public long Timestamp { get; set; }

    [JsonPropertyName("TYPE")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Type { get; set; }

    [JsonPropertyName("MARKET")]
    public string Market { get; set; }

    [JsonPropertyName("INSTRUMENT")]
    public string Instrument { get; set; }

    [JsonPropertyName("OPEN")]
    public double Open { get; set; }

    [JsonPropertyName("HIGH")]
    public double High { get; set; }

    [JsonPropertyName("LOW")]
    public double Low { get; set; }

    [JsonPropertyName("CLOSE")]
    public double Close { get; set; }

    [JsonPropertyName("FIRST_MESSAGE_TIMESTAMP")]
    public long FirstMessageTimestamp { get; set; }

    [JsonPropertyName("LAST_MESSAGE_TIMESTAMP")]
    public long LastMessageTimestamp { get; set; }

    [JsonPropertyName("FIRST_MESSAGE_VALUE")]
    public double FirstMessageValue { get; set; }

    [JsonPropertyName("HIGH_MESSAGE_VALUE")]
    public double HighMessageValue { get; set; }

    [JsonPropertyName("HIGH_MESSAGE_TIMESTAMP")]
    public long HighMessageTimestamp { get; set; }

    [JsonPropertyName("LOW_MESSAGE_VALUE")]
    public double LowMessageValue { get; set; }

    [JsonPropertyName("LOW_MESSAGE_TIMESTAMP")]
    public long LowMessageTimestamp { get; set; }

    [JsonPropertyName("LAST_MESSAGE_VALUE")]
    public double LastMessageValue { get; set; }

    [JsonPropertyName("TOTAL_INDEX_UPDATES")]
    public long TotalIndexUpdates { get; set; }

    [JsonPropertyName("VOLUME")]
    public double Volume { get; set; }

    [JsonPropertyName("QUOTE_VOLUME")]
    public double QuoteVolume { get; set; }

    [JsonPropertyName("VOLUME_TOP_TIER")]
    public double VolumeTopTier { get; set; }

    [JsonPropertyName("QUOTE_VOLUME_TOP_TIER")]
    public double QuoteVolumeTopTier { get; set; }

    [JsonPropertyName("VOLUME_DIRECT")]
    public double VolumeDirect { get; set; }

    [JsonPropertyName("QUOTE_VOLUME_DIRECT")]
    public double QuoteVolumeDirect { get; set; }

    [JsonPropertyName("VOLUME_TOP_TIER_DIRECT")]
    public double VolumeTopTierDirect { get; set; }

    [JsonPropertyName("QUOTE_VOLUME_TOP_TIER_DIRECT")]
    public double QuoteVolumeTopTierDirect { get; set; }

    public string Ticker => Instrument.Split('-')[1];
}