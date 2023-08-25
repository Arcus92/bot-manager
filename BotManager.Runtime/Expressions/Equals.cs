namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Returns a boolean value if <see cref="A"/> and <see cref="B"/> are equal.
/// </para>
/// <para>
/// Returns type os <see cref="bool"/>.
/// </para>
/// <example>
/// This json example returns <c>true</c>:
/// <code>
/// { "$Equals": { "A": 10, "B": 10 } }
/// </code>
/// This json example returns <c>false</c>:
/// <code>
/// { "$Equals": { "A": 10, "B": 100 } }
/// </code>
/// </example>
/// </summary>
[Serializable]
public sealed class Equals : IExpression
{
    /// <summary>
    /// Gets the first value
    /// </summary>
    public IExpression? A { get; set; }
    
    /// <summary>
    /// Gets the second value
    /// </summary>
    public IExpression? B { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var a = await context.ExecuteAsync<object?>(A);
        var b = await context.ExecuteAsync<object?>(B);

        return Equals(a, b);
    }
}