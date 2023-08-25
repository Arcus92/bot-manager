using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// A static <see cref="uint"/> value.
/// </para>
/// <para>
/// Return type is <see cref="uint"/>.
/// </para>
/// <example>
/// This json example returns <c>10</c>:
/// <code>
/// { "$UInt32": 10 }
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(UInt32Converter))]
[Serializable]
public sealed class UInt32 : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public UInt32()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="uint"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public UInt32(uint value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    [JsonRootProperty]
    public uint Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}