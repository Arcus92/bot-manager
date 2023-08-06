namespace BotManager.Runtime.Expressions;

/// <summary>
/// Sets a variable in the current <see cref="RuntimeContext"/>.
/// <para>
/// Returns: Type of the variable.
/// </para>
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
    [Input]
    public string Name { get; set; }

    /// <summary>
    /// Gets and sets the value to set.
    /// </summary>
    [Input]
    public IExpression? Value { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var value = await context.ExecuteAsync(Value, returnType);

        context.Set(Name, value);
        
        return value;
    }
}