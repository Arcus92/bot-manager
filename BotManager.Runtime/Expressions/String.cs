using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A static <see cref="string"/> value.
/// <para>
/// Returns: <see cref="string"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(StringConverter))]
public sealed class String : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public String()
    {
        Value = string.Empty;
    }
    
    /// <summary>
    /// Creates a <see cref="string"/> expression.
    /// </summary>
    /// <param name="value">The value.</param>
    public String(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the value.
    /// </summary>
    public string Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}