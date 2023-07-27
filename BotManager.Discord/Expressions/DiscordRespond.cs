using BotManager.Runtime;
using Discord;

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
    /// Gets and sets the expression to resolve the message.
    /// This can be a <see cref="string"/> or a <see cref="DiscordEmbed"/>. All other types are converted with
    /// <see cref="object.ToString"/>.
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

        var allowedMentions =
            AllowedMentions ? global::Discord.AllowedMentions.All : global::Discord.AllowedMentions.None;
        
        // Build the message
        var message = await context.ExecuteAsync<object?>(Message);
        switch (message)
        {
            case null:
                context.Logger.Error(DiscordInit.Tag, "Discord message is null.");
                return null;

            case Embed embed:
                await command.RespondAsync(embed: embed, allowedMentions: allowedMentions);
                return null;
            
            default:
                var text = message.ToString();
                await command.RespondAsync(text, allowedMentions: allowedMentions);
                return null;
        }
    }
}