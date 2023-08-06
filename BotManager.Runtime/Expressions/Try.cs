namespace BotManager.Runtime.Expressions;

/// <summary>
/// Trys to execute the <see cref="Expression"/> and handles any exceptions. If an exception is throw while executing
/// <see cref="Expression"/>, it will write the <see cref="Exception"/> to 'exception' in the current
/// <see cref="RuntimeContext"/> and then run the <see cref="Catch"/> expression.
/// <para>
/// Returns: Return value of the executed branch.
/// </para>
/// </summary>
public class Try : IExpression
{
    /// <summary>
    /// Gets and sets the expression to try.
    /// </summary>
    [Input]
    public IExpression? Expression { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to execute if <see cref="Expression"/> failed.
    /// </summary>
    [Input]
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