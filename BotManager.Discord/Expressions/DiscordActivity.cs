using System.Text.Json.Serialization;
using BotManager.Runtime;
using BotManager.Runtime.Converters;
using Discord;

namespace BotManager.Discord.Expressions;

/// <summary>
/// <para>
/// Sets the status activity shown in the current bot users profile. The <see cref="DiscordInit"/> must be initialized.
/// </para>
/// <para>
/// Return type is <c>null</c>.
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
    /// Gets and sets the activity type. See <see cref="ActivityType"/>.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ActivityType>))]
    public ActivityType Activity { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var discord = context.GetDiscord();
        if (discord is null)
            return null;

        // Resolve the name
        var name = await context.ExecuteAsync<string>(Name);
        var streamUrl = await context.ExecuteAsync<string>(StreamUrl);
        
        context.Logger.Info(DiscordInit.Tag, $"Setting Discord activity to [{Activity}] '{name}'...");
        
        await discord.Client.SetGameAsync(name, streamUrl: streamUrl, type: Activity);
        return null;
    }
}