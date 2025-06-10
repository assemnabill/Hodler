using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hodler.Application.Portfolios.Services.BitcoinBlockchain;

public class ScriptpubkeyTypeConverter : JsonConverter<ScriptPublicKeyType>
{
    public override ScriptPublicKeyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return ScriptPublicKeyType.Unknown;

        value = string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1));

        return Enum.TryParse<ScriptPublicKeyType>(value, true, out var result)
            ? result
            : ScriptPublicKeyType.Unknown;

    }

    public override void Write(Utf8JsonWriter writer, ScriptPublicKeyType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}