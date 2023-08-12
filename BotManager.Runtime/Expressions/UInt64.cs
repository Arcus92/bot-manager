using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A static <see cref="ulong"/> value.
/// <para>
/// Returns: <see cref="ulong"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(UInt64Converter))]
public sealed class UInt64 : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public UInt64()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="ulong"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public UInt64(ulong value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    [JsonRootProperty]
    public ulong Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}