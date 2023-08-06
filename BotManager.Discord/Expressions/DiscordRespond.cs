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
    /// </summary>
    [Input]
    public IExpression? Message { get; set; }
    
    /// <summary>
    /// Gets and sets the optional embed data for this message.
    /// </summary>
    [Input]
    public DiscordEmbed? Embed { get; set; }
    
    /// <summary>
    /// Gets and sets if mentions are allowed.
    /// </summary>
    [Input]
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
        var message = await context.ExecuteAsync<string?>(Message);
        var embed = Embed is null ? null : await Embed.BuildAsync(context);

        // Check empty message
        if (string.IsNullOrEmpty(message) && embed is null)
        {
            context.Logger.Error(DiscordInit.Tag, $"Discord message is empty.");
            return null;
        }
        
        await command.RespondAsync(message, embed: embed, allowedMentions: allowedMentions);
        return null;
    }
}