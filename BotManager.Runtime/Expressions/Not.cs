using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Inverts the given boolean expression.
/// <para>
/// Returns: <see cref="bool"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(NotConverter))]
public sealed class Not : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Not()
    {
    }
    
    /// <summary>
    /// Creates an expression that inverts the given <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression"></param>
    public Not(IExpression? expression)
    {
        Expression = expression;
    }
    
    /// <summary>
    /// Gets and sets the expression
    /// </summary>
    [JsonRootProperty]
    public IExpression? Expression { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Expression is null)
            return true;

        var value = await context.ExecuteAsync<bool>(Expression);
        return !value;
    }
}