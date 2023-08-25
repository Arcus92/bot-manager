namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Trys to execute the <see cref="Expression"/> and handles any exceptions. If an exception is throw while executing
/// <see cref="Expression"/>, it will write the <see cref="Exception"/> to 'exception' in the current
/// <see cref="RuntimeContext"/> and then run the <see cref="Catch"/> expression.
/// </para>
/// <para>
/// Returns the value of the executed branch.
/// </para>
/// <example>
/// This json example returns <c>10</c>:
/// <code>
/// { "$Try": { "Expression": 10, "Catch": -10 } }
/// </code>
/// </example>
/// </summary>
[Serializable]
public sealed class Try : IExpression
{
    /// <summary>
    /// Gets and sets the expression to try.
    /// </summary>
    public IExpression? Expression { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to execute if <see cref="Expression"/> failed.
    /// </summary>
    public IExpression? Catch { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        try
        {
            return await context.ExecuteAsync<object?>(Expression);
        }
        catch (Exception ex)
        {
            context.Set("exception", ex);
            return await context.ExecuteAsync<object?>(Catch);
        }
    }
}