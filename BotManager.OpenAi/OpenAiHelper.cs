using BotManager.OpenAi.Expressions;
using BotManager.Runtime;

namespace BotManager.OpenAi;

/// <summary>
/// A helper class for the OpenAI plugin.
/// </summary>
public static class OpenAiHelper
{
    /// <summary>
    /// Returns the global <see cref="OpenAiPlugin"/> from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>Returns the <see cref="OpenAiPlugin"/>. Returns <c>null</c> if OpenAI wasn't initialized.</returns>
    public static OpenAiPlugin? OpenAi(this RuntimeContext context)
    {
        return context.Get("openai.plugin") as OpenAiPlugin;
    }
}