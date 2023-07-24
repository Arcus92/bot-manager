namespace BotManager.Runtime.Expressions;

/// <summary>
/// Executes the <see cref="Then"/> expression if <see cref="Condition"/> returns <c>true</c>.
/// Otherwise <see cref="Else"/> is executed.
/// <para>
/// Returns: Return value of the executed branch.
/// </para>
/// </summary>
public sealed class If : IExpression
{
    /// <summary>
    /// Gets and sets the condition to check.
    /// </summary>
    public IExpression? Condition { get; set; }
    
    /// <summary>
    /// Gets and sets the operation if the condition is true.
    /// </summary>
    public IExpression? Then { get; set; }
    
    /// <summary>
    /// Gets and sets the operation if the condition is false.
    /// </summary>
    public IExpression? Else { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var condition = await context.ExecuteAsync<bool>(Condition);
        if (condition)
            return await context.ExecuteAsync(Then, returnType);
        return await context.ExecuteAsync(Else, returnType);
    }
}