using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// A static <see cref="bool"/> value.
/// </para>
/// <para>
/// Returns type is <see cref="bool"/>.
/// </para>
/// <example>
/// This json example returns <c>true</c>:
/// <code>
/// { "$Boolean": true }
/// </code>
/// Or even shorter:
/// <code>
/// true
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(BooleanConverter))]
[Serializable]
public sealed class Boolean : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Boolean()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="bool"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public Boolean(bool value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    [JsonRootProperty]
    public bool Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}