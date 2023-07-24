using System.Text.Json;
using System.Text.Json.Serialization;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The base <see cref="JsonConverter"/> for <see cref="IExpression"/> that take only one <see cref="IExpression"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ExpressionConverterBase<T> : JsonConverter<T>
{
    /// <summary>
    /// Creates the instance from the expression
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract T From(IExpression? value);

    /// <summary>
    /// Returns the expression from the instance
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract IExpression? To(T value);
    
    /// <inheritdoc />
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        var expression = JsonSerializer.Deserialize<IExpression?>(ref reader, options);
        if (expression is null)
            return default;

        return From(expression);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var expression = To(value);
        JsonSerializer.Serialize(writer, expression, options);
    }
}
