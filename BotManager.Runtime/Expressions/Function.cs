namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Registers a new function in the runtime context. Use <see cref="Call"/> to execute it later.
/// </para>
/// <para>
/// Return type is <c>null</c>.
/// </para>
/// <example>
/// This json example executes registers a user-defined function "MyFunction":
/// <code>
/// { "$Function": { "Name": "MyFunction", "Action": 10 } }
/// </code>
/// </example>
/// </summary>
[Serializable]
public sealed class Function : IExpression
{
    /// <summary>
    /// Gets and sets the name of the function.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets and sets the expression that is executed when the stored function is called.
    /// </summary>
    public IExpression? Action { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Name is null || Action is null)
            return Task.FromResult<object?>(null);
        
        context.Functions.Set(Name, Action);
        return Task.FromResult<object?>(null);
    }
}