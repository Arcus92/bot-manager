using System.Text.Json.Serialization;
using Discord;

namespace BotManager.Discord;

/// <summary>
/// Defines a option for a <see cref="DiscordSlashCommand"/>.
/// </summary>
public struct DiscordSlashCommandOption
{
    /// <summary>
    /// Gets and sets the option name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets and sets the option type.
    /// </summary>
    public ApplicationCommandOptionType Type { get; set; }

    /// <summary>
    /// Gets and sets the option description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets and sets if this option is required.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsRequired { get; set; }
}