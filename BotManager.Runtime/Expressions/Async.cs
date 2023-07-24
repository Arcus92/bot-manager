using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Runs the <see cref="Expression"/> in a new thread.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
[JsonConverter(typeof(AsyncConverter))]
public sealed class Async : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Async()
    {
    }
    
    /// <summary>
    /// Creates an expression that is runs another <paramref name="expression"/> in another thread.
    /// </summary>
    /// <param name="expression">The expression.</param>
    public Async(IExpression? expression)
    {
        Expression = expression;
    }
    
    /// <summary>
    /// Gets and sets the expression
    /// </summary>
    public IExpression? Expression { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Expression is null)
            return Task.FromResult<object?>(null);
        
        var newContext = new RuntimeContext(context);
        var expression = Expression;
        _ = Task.Run(() => newContext.ExecuteAsync(expression));
        return Task.FromResult<object?>(null);
    }
}