using System.Text.Json;
using System.Text.Json.Serialization;
using UInt64 = BotManager.Runtime.Expressions.UInt64;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Expressions.UInt64"/>.
/// </summary>
public class UInt64Converter : JsonConverter<UInt64>
{
    /// <inheritdoc />
    public override UInt64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'Number'.");
        return new UInt64(reader.GetUInt64());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, UInt64 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}