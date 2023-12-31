using System.ComponentModel;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Holds execution and waits the defined number of milliseconds.
/// </para>
/// <para>
/// Return type is <c>null</c>.
/// </para>
/// <example>
/// This json example waits three seconds and returns <c>null</c>:
/// <code>
/// { "$Delay": 3000 }
/// </code>
/// </example>
/// </summary>
[TypeConverter(typeof(DelayConverter))]
[Serializable]
public sealed class Delay : IExpression
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
    [JsonRootProperty]
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