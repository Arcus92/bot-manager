using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// A static <see cref="long"/> value.
/// </para>
/// <para>
/// Return type is <see cref="long"/>.
/// </para>
/// <example>
/// This json example returns <c>10</c>:
/// <code>
/// { "$Int64": 10 }
/// </code>
/// </example>
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