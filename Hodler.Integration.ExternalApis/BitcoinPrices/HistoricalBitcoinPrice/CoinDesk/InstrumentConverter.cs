using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Hodler.Domain.Shared.Models;

namespace Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;

internal partial class InstrumentConverter : JsonConverter<FiatCurrency>
{
    public static readonly InstrumentConverter Singleton = new();

    [GeneratedRegex(@"^BTC-[A-Z]{3,4}$")]
    private static partial Regex InstrumentRegex();

    public override bool CanConvert(Type t) => t == typeof(FiatCurrency);

    public override FiatCurrency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (value == null || !InstrumentRegex().IsMatch(value))
            throw new Exception("Error while converting Coin Desk Api Response: Cannot unmarshal type Instrument");

        var ticker = value.Split('-')[1];
        return FiatCurrency.GetByTicker(ticker);

    }

    public override void Write(Utf8JsonWriter writer, FiatCurrency value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, $"BTC-{value.Ticker}", options);
    }
}