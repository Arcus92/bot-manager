using System.Text.Json;
using System.Text.Json.Serialization;
using UInt32 = BotManager.Runtime.Expressions.UInt32;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Expressions.UInt32"/>.
/// </summary>
public sealed class UInt32Converter : JsonConverter<UInt32>
{
    /// <inheritdoc />
    public override UInt32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'Number'.");
        return new UInt32(reader.GetUInt32());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, UInt32 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}