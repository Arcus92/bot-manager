using BotManager.Runtime;

namespace BotManager.Discord;

/// <summary>
/// Static plugin class for the Discord extension.
/// </summary>
public static class DiscordPlugin
{
    /// <summary>
    /// Registers all expressions from the Discord plugin.
    /// </summary>
    public static void Register()
    {
        IExpression.RegisterExpressionTypesFromAssembly(typeof(DiscordPlugin).Assembly);
    }
}