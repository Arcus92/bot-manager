using BotManager.Runtime;

namespace BotManager.Discord.Expressions;

/// <summary>
/// Responds with a text message on a <see cref="DiscordSlashCommand"/>. The <see cref="DiscordInit"/> must be
/// initialized.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
public sealed class DiscordRespond : IExpression
{
    /// <summary>
    /// Gets and sets the message.
    /// </summary>
    public IExpression? Message { get; set; }
    
    /// <summary>
    /// Gets and sets if mentions are allowed.
    /// </summary>
    public bool AllowedMentions { get; set; }
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var command = context.GetDiscordCommand();
        if (command is null)
        {
            context.Logger.Error(DiscordInit.Tag, "Could not detect Discord slash command to respond to.");
            return null;
        }

        // Build the message
        var message = await context.ExecuteAsync<string>(Message);
        if (string.IsNullOrEmpty(message))
        {
            context.Logger.Error(DiscordInit.Tag, $"Discord message is empty.");
            return null;
        }
        
        await command.RespondAsync(message,
            allowedMentions: AllowedMentions ? global::Discord.AllowedMentions.All : global::Discord.AllowedMentions.None);
        return null;
    }
}