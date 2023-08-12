using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Returns the value of a variable from the current <see cref="RuntimeContext"/>.
/// <para>
/// Returns: Type of the variable.
/// </para>
/// </summary>
[JsonConverter(typeof(GetConverter))]
public sealed class Get : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Get()
    {
        Name = string.Empty;
    }
    
    /// <summary>
    /// Creates an expression that returns a variable from the current <see cref="RuntimeContext"/>. 
    /// </summary>
    /// <param name="name">The variable name.</param>
    public Get(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// Gets and sets the name of the variable
    /// </summary>
    [JsonRootProperty]
    public string Name { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult((object?)context.Get(Name));
    }
}