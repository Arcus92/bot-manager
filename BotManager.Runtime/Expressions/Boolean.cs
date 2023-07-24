using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A static <see cref="bool"/> value.
/// <para>
/// Returns: <see cref="bool"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(BooleanConverter))]
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
    public bool Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}