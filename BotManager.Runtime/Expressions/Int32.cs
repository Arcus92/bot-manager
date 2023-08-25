using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// A static <see cref="int"/> value.
/// </para>
/// <para>
/// Return type is <see cref="int"/>.
/// </para>
/// <example>
/// This json example returns <c>10</c>:
/// <code>
/// { "$Int32": 10 }
/// </code>
/// Or even shorter:
/// <code>
/// 10
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(Int32Converter))]
[Serializable]
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