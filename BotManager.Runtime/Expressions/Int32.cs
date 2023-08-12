using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A static <see cref="int"/> value.
/// <para>
/// Returns: <see cref="int"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(Int32Converter))]
public sealed class Int32 : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Int32()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="int"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public Int32(int value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    [JsonRootProperty]
    public int Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}