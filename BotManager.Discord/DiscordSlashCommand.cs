using BotManager.Runtime;

namespace BotManager.Discord;

/// <summary>
/// Defines the action for a Discord slash command. A slash command can be global or set to a specific guild.
/// </summary>
public sealed class DiscordSlashCommand
{
    /// <summary>
    /// Gets and sets the command name (without the leading slash).
    /// </summary>
    [Input]
    public string? Name { get; set; }

    /// <summary>
    /// Gets and sets the commands description.
    /// </summary>
    [Input]
    public string? Description { get; set; }

    /// <summary>
    /// Gets and sets the guild for this command.
    /// If no guild is defined, it will be registered as global command.
    /// </summary>
    [Input]
    public DiscordIdentifier? Guild { get; set; }

    /// <summary>
    /// Gets and sets the list of parameter for this command.
    /// </summary>
    [Input]
    public DiscordSlashCommandOption[]? Options { get; set; }

    /// <summary>
    /// Gets and sets the expression to run once a user executes this slash command.
    /// </summary>
    [Input]
    public IExpression? Action { get; set; }
}