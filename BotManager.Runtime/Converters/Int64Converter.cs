using System.Text.Json;
using System.Text.Json.Serialization;
using Int64 = BotManager.Runtime.Expressions.Int64;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Expressions.Int64"/>.
/// </summary>
public class Int64Converter : JsonConverter<Int64>
{
    /// <inheritdoc />
    public override Int64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'Number'.");
        return new Int64(reader.GetInt64());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Int64 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}