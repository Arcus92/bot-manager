using System.Text.Json;
using System.Text.Json.Serialization;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The generic <see cref="JsonConverter"/> for <see cref="Expressions.List"/>s.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ListConverter<T> : JsonConverter<T> where T : class, IList<IExpression?>, new()
{
    /// <inheritdoc />
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return StaticRead(ref reader, options);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        StaticWrite(writer, value, options);
    }

    /// <summary>
    /// Reads an <see cref="Expressions.List"/> type from json.
    /// </summary>
    /// <param name="reader">The json reader.</param>
    /// <param name="options">The json options.</param>
    /// <returns>Returns the list instance.</returns>
    public static T? StaticRead(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        // Handle null
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'StartArray'.");

        var list = new T();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            var item = JsonSerializer.Deserialize<IExpression?>(ref reader, options);
            list.Add(item);
        }
        
        return list;
    }

    /// <summary>
    /// Writes the <see cref="Expressions.List"/> to json.
    /// </summary>
    /// <param name="writer">The json writer.</param>
    /// <param name="value">The list instance.</param>
    /// <param name="options">The json options.</param>
    public static void StaticWrite(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        // Handle null
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();
    }
}