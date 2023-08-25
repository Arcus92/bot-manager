using BotManager.Discord.Models;
using BotManager.Runtime;
using Discord;

namespace BotManager.Discord.Expressions;

/// <summary>
/// <para>
/// Sends a text message to a Discord channel. The <see cref="DiscordInit"/> must be initialized.
/// </para>
/// <para>
/// Return type is <c>null</c>.
/// </para>
/// <example>
/// This json example sends a Discord message to a channel and returns <c>null</c>:
/// <code>
/// { "$DiscordSend": { "Message": "Hello World!", "Guild": "MyServer", "Channel": "MyChannel" } }
/// </code>
/// </example>
/// </summary>
[Serializable]
public sealed class DiscordSend : IExpression
{
    /// <summary>
    /// Gets and sets the expression to resolve the message.
    /// </summary>
    public IExpression? Message { get; set; }
    
    /// <summary>
    /// Gets and sets the optional embed data for this message.
    /// </summary>
    public DiscordEmbed? Embed { get; set; }

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
        var message = await context.ExecuteAsync<string?>(Message);
        var embed = Embed is null ? null : await Embed.BuildAsync(context);

        // Check empty message
        if (string.IsNullOrEmpty(message) && embed is null)
        {
            context.Logger.Error(DiscordInit.Tag, $"Discord message is empty.");
            return null;
        }
        
        await textChannel.SendMessageAsync(message, embed: embed, allowedMentions: allowedMentions);
        return null;
    }
}