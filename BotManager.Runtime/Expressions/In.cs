namespace BotManager.Runtime.Expressions;

/// <summary>
/// Returns a boolean value if <see cref="Value"/> is equal to at least one item in <see cref="List"/>.
/// <para>
/// Returns: <see cref="bool"/> value.
/// </para>
/// </summary>
public sealed class In : IExpression
{
    /// <summary>
    /// Gets the value to check.
    /// </summary>
    public IExpression? Value { get; set; }
    
    /// <summary>
    /// Gets the list to compare <see cref="Value"/> with.
    /// </summary>
    public List? List { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var value = await context.ExecuteAsync<object?>(Value);
        if (List is null)
            return false;

        foreach (var item in List)
        {
            var other = await context.ExecuteAsync<object?>(item);
            if (Equals(value, other))
                return true;
        }
        
        return false;
    }
}