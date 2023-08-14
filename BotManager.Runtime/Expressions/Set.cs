namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Sets a variable in the current <see cref="RuntimeContext"/>.
/// </para>
/// <para>
/// Returns type is the type of the variable.
/// </para>
/// <example>
/// This json example set the variable "MyVar" to <c>10</c> and returns this value:
/// <code>
/// { "$Set": { "Name": "MyVar", "Value": 10 } }
/// </code>
/// </example>
/// </summary>
public sealed class Set : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Set()
    {
        Name = string.Empty;
    }
    
    /// <summary>
    /// Creates an expression that sets a variable to the given <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The variable name.</param>
    /// <param name="value">The value.</param>
    public Set(string name, IExpression? value)
    {
        Name = name;
        Value = value;
    }
    
    /// <summary>
    /// Gets and sets the name of the variable to set.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets and sets the value to set.
    /// </summary>
    public IExpression? Value { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var value = await context.ExecuteAsync(Value, returnType);

        context.Set(Name, value);
        
        return value;
    }
}