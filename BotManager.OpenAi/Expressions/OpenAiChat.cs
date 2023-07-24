using BotManager.Runtime;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace BotManager.OpenAi.Expressions;

/// <summary>
/// Executes a char request to the OpenAI api. The <see cref="OpenAiInit"/> must be initialized.
/// <para>
/// Returns: <see cref="string"/> value.
/// </para>
/// </summary>
public sealed class OpenAiChat : IExpression
{
    /// <summary>
    /// The chat messages to respond to.
    /// <para>
    /// You should provide your ruleset as a separate <see cref="StaticValues.ChatMessageRoles.System"/> message and not
    /// concat it with user input. Use a message with <see cref="StaticValues.ChatMessageRoles.User"/> role for user
    /// input. This prioritizes your ruleset over the user input and protects against prompt injection.
    /// </para>
    /// </summary>
    public OpenAiChatMessage[]? Messages { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var openAi = context.OpenAi();
        if (openAi is null)
            return null;

        // Builds the messages
        var messages = new List<ChatMessage>();
        if (Messages is null || Messages.Length == 0)
            return null;

        context.Logger.Info(OpenAiInit.Tag, "Sending OpenAI chat request...");
        
        foreach (var message in Messages)
        {
            var text = await context.ExecuteAsync<string>(message.Text);
            if (string.IsNullOrEmpty(text))
                continue;
            messages.Add(new ChatMessage(message.Role, text));
        }
        
        // Builds the request
        var request = new ChatCompletionCreateRequest
        {
            Messages = messages,
            Model = Models.ChatGpt3_5Turbo
        };

        // Fetches the result
        var response = await openAi.Service.CreateCompletion(request);
        if (!response.Successful)
        {
            context.Logger.Error(OpenAiInit.Tag, "OpenAI chat request failed!");
            return null;
        }

        var answer = response.Choices.First();
        return answer.Message.Content;
    }
}