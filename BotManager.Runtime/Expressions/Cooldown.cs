namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Executes the <see cref="Action"/> expression with a cooldown of <see cref="Milliseconds"/>. If this expression is
/// called multiple times while the cooldown timer is still ticking down, then the <see cref="Fallback"/> branch is
/// executed instead.
/// </para>
/// <para>
/// The cooldown timer is started before <see cref="Action"/> is executed. The time <see cref="Action"/> takes to run
/// is ignored in the cooldown timer.
/// </para>
/// <para>
/// Returns the value of the executed branch.
/// </para>
/// <example>
/// This json example will return <c>10</c>, but when called again withing 5 minutes will return <c>0</c>:
/// <code>
/// { "$Cooldown": { "Action": 10, "Fallback": 0, "Milliseconds": 300000 } }
/// </code>
/// </example>
/// </summary>
[Serializable]
public class Cooldown : IExpression
{
    /// <summary>
    /// Gets and sets the main expression to execute when no cooldown timer is ticking.
    /// </summary>
    public IExpression? Action { get; set; }
    
    /// <summary>
    /// Gets and sets the optional fallback expression that is executed when a the cooldown timer is still ticking down.
    /// </summary>
    public IExpression? Fallback { get; set; }
    
    /// <summary>
    /// Gets and sets the number of milliseconds for the cooldown timer.
    /// </summary>
    public IExpression? Milliseconds { get; set; }

    /// <summary>
    /// The time of the last execution of the expression.
    /// </summary>
    private DateTime? _lastExecution;
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        // No cooldown time provided? Just execute the action anytime.
        if (Milliseconds is null)
        {
            return await context.ExecuteAsync(Action, returnType);
        }
        
        // Resolve the cooldown timer
        var milliseconds = await context.ExecuteAsync<int>(Milliseconds);
        var cooldownSpan = TimeSpan.FromMilliseconds(milliseconds);
        var now = context.DateTimeProvider.Now;

        // Cooldown is still ticking...
        if (_lastExecution.HasValue && _lastExecution > now - cooldownSpan)
            return await context.ExecuteAsync(Fallback, returnType);
        
        _lastExecution = now;
        return await context.ExecuteAsync(Action, returnType);

    }
}