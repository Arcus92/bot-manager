using System.Text.Json;
using System.Text.Json.Serialization;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The base <see cref="JsonConverter"/> for <see cref="IExpression"/> that take only one <see cref="string"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StringConverterBase<T> : JsonConverter<T>
{
    /// <summary>
    /// Creates the instance from the string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract T From(string value);

    /// <summary>
    /// Returns the string from the instance.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract string To(T value);
    
    /// <inheritdoc />
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Unexpected token '{reader.TokenType}'. Expected 'String'.");

        var name = reader.GetString();
        if (name is null)
            return default;

        return From(name);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        
        writer.WriteStringValue(To(value));
    }
}