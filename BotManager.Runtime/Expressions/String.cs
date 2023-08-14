using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// A static <see cref="string"/> value.
/// </para>
/// <para>
/// Returns type is <see cref="string"/>.
/// </para>
/// <example>
/// This json example returns <c>"Hello"</c>:
/// <code>
/// { "$String": "Hello" }
/// </code>
/// Or even shorter:
/// <code>
/// "Hello"
/// </code>
/// </example>
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
    [JsonRootProperty]
    public string Value { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Value);
    }
}