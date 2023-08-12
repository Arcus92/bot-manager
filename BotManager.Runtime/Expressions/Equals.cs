namespace BotManager.Runtime.Expressions;

/// <summary>
/// Returns a boolean value if <see cref="A"/> and <see cref="B"/> are equal.
/// <para>
/// Returns: <see cref="bool"/> value.
/// </para>
/// </summary>
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