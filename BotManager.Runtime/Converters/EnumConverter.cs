using System.Text.Json;
using System.Text.Json.Serialization;

namespace BotManager.Runtime.Converters;

/// <summary>
/// A generic <see cref="JsonConverter"/> for enum types. It can read enums by its number value or by its value name.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class EnumConverter<T> : JsonConverter<T> where T : struct, IConvertible
{
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeof(T);
        if (!type.IsEnum)
            throw new ArgumentException($"Unexpected T type in EnumConverter '{type.Name}'. Expected 'enum'.");
        
        // Handle numbers as enum index
        if (reader.TokenType == JsonTokenType.Number)
        {
            var i = reader.GetInt32();
            return (T)(object)i;
        }
        
        // Handle string as enum name
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (s is null)
                return default;

            if (Enum.TryParse<T>(s, out var value))
                return value;

            throw new ArgumentException($"Unknown value '{s}' for '{type.Name}'.");
        }

        throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'String' or 'Number'.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var type = typeof(T);
        if (!type.IsEnum)
            throw new ArgumentException($"Unexpected T type in EnumConverter '{type.Name}'. Expected 'enum'.");
        
        // We may want to handle flags differently in future.
        writer.WriteStringValue(value.ToString());
    }
}