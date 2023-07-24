using BotManager.Runtime;

namespace BotManager.OpenAi;

/// <summary>
/// Static plugin class for the OpenAi extension.
/// </summary>
public static class OpenAiPlugin
{
    /// <summary>
    /// Registers all expressions from the OpenAI plugin.
    /// </summary>
    public static void Register()
    {
        IExpression.RegisterExpressionTypesFromAssembly(typeof(OpenAiPlugin).Assembly);
    }
}