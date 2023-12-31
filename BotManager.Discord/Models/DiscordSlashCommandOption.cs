using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;
using Discord;

namespace BotManager.Discord.Models;

/// <summary>
/// Defines a option for a <see cref="DiscordSlashCommand"/>.
/// </summary>
[Serializable]
public struct DiscordSlashCommandOption
{
    /// <summary>
    /// Gets and sets the option name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets and sets the option type. See <see cref="ApplicationCommandOptionType"/>.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ApplicationCommandOptionType>))]
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