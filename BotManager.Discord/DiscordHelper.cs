using BotManager.Discord.Expressions;
using BotManager.Runtime;
using Discord;
using Discord.WebSocket;

namespace BotManager.Discord;

/// <summary>
/// A helper class for the Discord plugin.
/// </summary>
public static class DiscordHelper
{
    /// <summary>
    /// Returns the global <see cref="DiscordPlugin"/> from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>Returns the <see cref="DiscordPlugin"/>. Returns <c>null</c> if Discord wasn't initialized.</returns>
    public static DiscordPlugin? Discord(this RuntimeContext context)
    {
        return context.Get("discord.plugin") as DiscordPlugin;
    }

    /// <summary>
    /// Returns the identifier by an operation
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static async Task<DiscordIdentifier?> DiscordIdentifierAsync(this RuntimeContext context, IExpression? expression)
    {
        var value = await context.ExecuteAsync<object?>(expression);
        if (value is null)
            return null;
        return new DiscordIdentifier(value);
    }
    
    /// <summary>
    /// Returns the Discord guild from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns></returns>
    public static IGuild? DiscordGuild(this RuntimeContext context)
    {
        return context.Get("discord.guild") as IGuild;
    }
    
    /// <summary>
    /// Returns the Discord guild from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="guild"></param>
    /// <returns></returns>
    public static async Task<IGuild?> DiscordGuildAsync(this RuntimeContext context, IExpression? guild)
    {
        if (guild is null)
            return DiscordGuild(context);
        
        var id = await DiscordIdentifierAsync(context, guild);
        if (id is null)
            return null;
        
        var discord = Discord(context);
        return discord?.GetGuild(id.Value);
    }
    
    /// <summary>
    /// Returns the Discord channel from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns></returns>
    public static IChannel? DiscordChannel(this RuntimeContext context)
    {
        return context.Get("discord.channel") as IChannel;
    }

    /// <summary>
    /// Returns the Discord channel from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="guild"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    public static async Task<IChannel?> DiscordChannelAsync(this RuntimeContext context, IExpression? guild, IExpression? channel)
    {
        // No channel defined. We use the channel from the current context
        if (channel is null)
            return DiscordChannel(context);
        
        // Get the guild first
        var discordGuild = await DiscordGuildAsync(context, guild);
        if (discordGuild is null)
            return null;

        var id = await DiscordIdentifierAsync(context, channel);
        if (id is null)
            return null;
        
        var channels = await discordGuild.GetChannelsAsync();
        var discordChanel = id.Value.GetChannel(channels);
        return discordChanel;
    }
    
    /// <summary>
    /// Returns the Discord user from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns></returns>
    public static IUser? DiscordUser(this RuntimeContext context)
    {
        return context.Get("discord.user") as IUser;
    }
    
    /// <summary>
    /// Returns the Discord slash command from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns></returns>
    public static SocketSlashCommand? DiscordCommand(this RuntimeContext context)
    {
        return context.Get("discord.command") as SocketSlashCommand;
    }
}