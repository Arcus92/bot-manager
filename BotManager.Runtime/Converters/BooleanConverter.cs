using System.Text.Json;
using System.Text.Json.Serialization;
using Boolean = BotManager.Runtime.Expressions.Boolean;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Boolean"/>.
/// </summary>
public class BooleanConverter : JsonConverter<Boolean>
{
    /// <inheritdoc />
    public override Boolean Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.True && reader.TokenType != JsonTokenType.False)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'True' or 'False'.");
        
        return new Boolean(reader.TokenType == JsonTokenType.True);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Boolean value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.Value);
    }
}