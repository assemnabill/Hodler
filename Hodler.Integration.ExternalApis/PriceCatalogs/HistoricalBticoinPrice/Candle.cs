using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBticoinPrice;

/// <summary>
/// This data is represented in OHLCV candles - Open, High, Low, Close, and Volume.
/// </summary>
public class Candle
{
    /// <summary>
    /// the price (quote) at which the first transaction was completed in a given time period
    /// </summary>
    [JsonPropertyName("open")]
    public string Open { get; set; }

    /// <summary>
    /// the top price (quote) at which the base was traded during the time period
    /// </summary>
    [JsonPropertyName("high")]
    public string High { get; set; }

    /// <summary>
    /// the bottom price (quote) at which the base was traded during the time period
    /// </summary>
    [JsonPropertyName("low")]
    public string Low { get; set; }

    /// <summary>
    /// the price (quote) at which the last transaction was completed in a given time period
    /// </summary>
    [JsonPropertyName("close")]
    public string Close { get; set; }

    /// <summary>
    /// the amount of base asset traded in the given time period
    /// </summary>
    [JsonPropertyName("volume")]
    public string Volume { get; set; }

    /// <summary>
    /// timestamp for starting of that time period, represented in UNIX milliseconds
    /// </summary>
    [JsonPropertyName("period")]
    public long Period { get; set; }
}