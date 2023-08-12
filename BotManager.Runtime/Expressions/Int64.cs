using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A static <see cref="long"/> value.
/// <para>
/// Returns: <see cref="long"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(Int64Converter))]
public sealed class Int64 : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Int64()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="int"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public Int64(long value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    [JsonRootProperty]
    public long Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}