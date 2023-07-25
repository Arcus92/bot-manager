using BotManager.Runtime;
using Discord;

namespace BotManager.Discord.Expressions;

/// <summary>
/// Sets the status activity shown in the current bot users profile. The <see cref="DiscordInit"/> must be initialized.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
public sealed class DiscordActivity : IExpression
{
    /// <summary>
    /// Gets and sets the expression to resolve the activity name.
    /// </summary>
    public IExpression? Name { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the stream url of that activity.
    /// </summary>
    public IExpression? StreamUrl { get; set; }

    /// <summary>
    /// Gets and sets the activity type.
    /// </summary>
    public ActivityType ActivityType { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var discord = context.GetDiscord();
        if (discord is null)
            return null;

        // Resolve the name
        var name = await context.ExecuteAsync<string>(Name);
        var streamUrl = await context.ExecuteAsync<string>(StreamUrl);
        
        await discord.Client.SetGameAsync(name, streamUrl: streamUrl, type: ActivityType);
        return null;
    }
}