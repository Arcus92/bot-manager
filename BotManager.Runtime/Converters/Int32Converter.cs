using System.Text.Json;
using System.Text.Json.Serialization;
using Int32 = BotManager.Runtime.Expressions.Int32;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Expressions.Int32"/>.
/// </summary>
public sealed class Int32Converter : JsonConverter<Int32>
{
    /// <inheritdoc />
    public override Int32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'Number'.");
        return new Int32(reader.GetInt32());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Int32 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}