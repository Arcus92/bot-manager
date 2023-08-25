using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Inverts the given boolean expression.
/// </para>
/// <para>
/// Return type is <see cref="bool"/>.
/// </para>
/// <example>
/// This json example returns <c>false</c>:
/// <code>
/// { "$Not": true }
/// </code>
/// This json example returns <c>true</c>:
/// <code>
/// { "$Not": false }
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(NotConverter))]
[Serializable]
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