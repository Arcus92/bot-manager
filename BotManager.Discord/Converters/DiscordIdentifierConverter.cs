using System.Text.Json;
using System.Text.Json.Serialization;

namespace BotManager.Discord.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="DiscordIdentifier"/>.
/// </summary>
public class DiscordIdentifierConverter : JsonConverter<DiscordIdentifier>
{
    /// <inheritdoc />
    public override DiscordIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle name
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (s is null) return default;
            return new DiscordIdentifier(s);
        }
        
        // Handle id
        if (reader.TokenType == JsonTokenType.Number)
        {
            var ul = reader.GetUInt64();
            return new DiscordIdentifier(ul);
        }

        throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'String' or 'Number'.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DiscordIdentifier value, JsonSerializerOptions options)
    {
        // Handle name
        if (value.Name is not null)
        {
            writer.WriteStringValue(value.Name);
            return;
        }

        // Handle id
        if (value.Id != 0)
        {
            writer.WriteNumberValue(value.Id);
            return;
        }
        
        writer.WriteNullValue();
    }
}