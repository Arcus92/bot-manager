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
    /// Returns the global <see cref="DiscordInit"/> from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>Returns the <see cref="DiscordInit"/>. Returns <c>null</c> if Discord wasn't initialized.</returns>
    public static DiscordInit? GetDiscord(this RuntimeContext context)
    {
        return context.Get("discord.plugin") as DiscordInit;
    }

    /// <summary>
    /// Returns the identifier by an operation
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="expression">The expression to resolve the id or name.</param>
    /// <returns>Returns the identifier with either id or name set.</returns>
    public static async Task<DiscordIdentifier?> GetDiscordIdentifierAsync(this RuntimeContext context, IExpression? expression)
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
    /// <returns>Returns the guild, if found.</returns>
    public static IGuild? GetDiscordGuild(this RuntimeContext context)
    {
        return context.Get("discord.guild") as IGuild;
    }
    
    /// <summary>
    /// Returns the Discord guild from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="guild">The expression to resolve the guid id or name.</param>
    /// <returns>Returns the guild, if found.</returns>
    public static async Task<IGuild?> GetDiscordGuildAsync(this RuntimeContext context, IExpression? guild)
    {
        if (guild is null)
            return GetDiscordGuild(context);
        
        var id = await GetDiscordIdentifierAsync(context, guild);
        if (id is null)
            return null;
        
        var discord = GetDiscord(context);
        return discord?.GetGuild(id.Value);
    }
    
    /// <summary>
    /// Returns the Discord channel from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>Returns the channel, if found.</returns>
    public static IChannel? GetDiscordChannel(this RuntimeContext context)
    {
        return context.Get("discord.channel") as IChannel;
    }

    /// <summary>
    /// Returns the Discord channel from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="guild">The expression to resolve the guid id or name.</param>
    /// <param name="channel">The expression to resolve the channel id or name.</param>
    /// <returns>Returns the channel, if found.</returns>
    public static async Task<IChannel?> GetDiscordChannelAsync(this RuntimeContext context, IExpression? guild, IExpression? channel)
    {
        // No channel defined. We use the channel from the current context
        if (channel is null)
            return GetDiscordChannel(context);
        
        // Get the guild first
        var discordGuild = await GetDiscordGuildAsync(context, guild);
        if (discordGuild is null)
            return null;

        var id = await GetDiscordIdentifierAsync(context, channel);
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
    /// <returns>Returns the user, if found.</returns>
    public static IUser? GetDiscordUser(this RuntimeContext context)
    {
        return context.Get("discord.user") as IUser;
    }
    
    /// <summary>
    /// Returns the Discord slash command from the current context.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>Returns the slash command, if found.</returns>
    public static SocketSlashCommand? GetDiscordCommand(this RuntimeContext context)
    {
        return context.Get("discord.command") as SocketSlashCommand;
    }
}