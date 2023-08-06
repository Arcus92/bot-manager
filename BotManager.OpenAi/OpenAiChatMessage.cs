using BotManager.OpenAi.Expressions;
using BotManager.Runtime;
using OpenAI.GPT3.ObjectModels;

namespace BotManager.OpenAi;

/// <summary>
/// Defines a single chat message for the <see cref="OpenAiChat"/> expression.
/// </summary>
public sealed class OpenAiChatMessage
{
    /// <summary>
    /// Gets and sets the role of the message.
    /// See <see cref="StaticValues.ChatMessageRoles"/>.
    /// </summary>
    [Input]
    public string Role { get; set; } = StaticValues.ChatMessageRoles.System;

    /// <summary>
    /// Gets and sets the message text.
    /// </summary>
    [Input]
    public IExpression? Text { get; set; }
}