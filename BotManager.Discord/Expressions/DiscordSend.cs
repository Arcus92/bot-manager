using BotManager.Runtime;
using Discord;

namespace BotManager.Discord.Expressions;

/// <summary>
/// Sends a text message to a Discord channel. The <see cref="DiscordInit"/> must be initialized.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
public sealed class DiscordSend : IExpression
{
    /// <summary>
    /// Gets and sets the expression to resolve the message.
    /// This can be a <see cref="string"/> or a <see cref="DiscordEmbed"/>. All other types are converted with
    /// <see cref="object.ToString"/>.
    /// </summary>
    public IExpression? Message { get; set; }

    /// <summary>
    /// Gets and sets if user, channel and role mentions are allowed in this message.
    /// <para>
    /// Use <c>&lt;@userId&gt;</c> to tag a user, <c>&lt;#channelId&gt;</c> to tag a channel and
    /// <c>@&lt;#roleId&gt;</c> to tag a role.
    /// </para>
    /// </summary>
    public bool AllowedMentions { get; set; }

    /// <summary>
    /// Gets and sets the target guild to send the message to. If not defined, it will use the guild from the current
    /// runtime context.
    /// </summary>
    public IExpression? Guild { get; set; }
    
    /// <summary>
    /// Gets and sets the target channel to send the message to. If not defined, it will use the channel from the
    /// current runtime context.
    /// </summary>
    public IExpression? Channel { get; set; }
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var channel = await context.GetDiscordChannelAsync(Guild, Channel);
        if (channel is null)
        {
            context.Logger.Error(DiscordInit.Tag, "Could not detect Discord channel to send message.");
            return null;
        }
        
        if (channel is not ITextChannel textChannel)
        {
            context.Logger.Error(DiscordInit.Tag, $"Discord channel '{channel.Name}' is not a text channel to send message to.");
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
                await textChannel.SendMessageAsync(embed: embed, allowedMentions: allowedMentions);
                return null;
            
            default:
                var text = message.ToString();
                await textChannel.SendMessageAsync(text, allowedMentions: allowedMentions);
                return null;
        }
    }
}