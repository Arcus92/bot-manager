using System.ComponentModel;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Holds execution and waits the defined number of milliseconds.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
[TypeConverter(typeof(DelayConverter))]
public class Delay : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Delay()
    {
    }
    
    /// <summary>
    /// Creates an expression that holds execution and waits the defined number of milliseconds.
    /// </summary>
    /// <param name="milliseconds">The expression to resolve the number of milliseconds to wait.</param>
    public Delay(IExpression? milliseconds)
    {
        Milliseconds = milliseconds;
    }
    
    /// <summary>
    /// Gets and sets the expression to resolve the number of milliseconds to wait.
    /// </summary>
    [Input(ContentProperty = true)]
    public IExpression? Milliseconds { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        // Don't wait
        if (Milliseconds is null)
            return null;

        // Resolve the time
        var milliseconds = await context.ExecuteAsync<int>(Milliseconds);
        
        await Task.Delay(milliseconds);
        return null;
    }
}