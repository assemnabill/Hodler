using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";


    public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
    private CultureInfo? _culture;
    private string? _dateTimeFormat;

    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;

    public DateTimeStyles DateTimeStyles
    {
        get => _dateTimeStyles;
        set => _dateTimeStyles = value;
    }

    public string? DateTimeFormat
    {
        get => _dateTimeFormat ?? string.Empty;
        set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
    }

    public CultureInfo Culture
    {
        get => _culture ?? CultureInfo.CurrentCulture;
        set => _culture = value;
    }

    public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        string text;

        if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
            || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
        {
            value = value.ToUniversalTime();
        }

        text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

        writer.WriteStringValue(text);
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateText = reader.GetString();

        if (string.IsNullOrEmpty(dateText) == false)
        {
            return !string.IsNullOrEmpty(_dateTimeFormat)
                ? DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles)
                : DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
        }

        return default;
    }
}