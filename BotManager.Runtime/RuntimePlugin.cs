namespace BotManager.Runtime;

/// <summary>
/// The runtime plugin hosts the <see cref="IExpression"/> runtime.
/// </summary>
public static class RuntimePlugin
{
    /// <summary>
    /// Registers all expressions from the runtime plugin.
    /// </summary>
    public static void Register()
    {
        IExpression.RegisterExpressionTypesFromAssembly(typeof(RuntimePlugin).Assembly);
    }
}