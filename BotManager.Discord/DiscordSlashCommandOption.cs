using System.Text.Json.Serialization;
using BotManager.Runtime;
using BotManager.Runtime.Converters;
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
    [Input]
    public string? Name { get; set; }

    /// <summary>
    /// Gets and sets the option type. See <see cref="ApplicationCommandOptionType"/>.
    /// </summary>
    [Input]
    [JsonConverter(typeof(EnumConverter<ApplicationCommandOptionType>))]
    public ApplicationCommandOptionType Type { get; set; }

    /// <summary>
    /// Gets and sets the option description.
    /// </summary>
    [Input]
    public string? Description { get; set; }

    /// <summary>
    /// Gets and sets if this option is required.
    /// </summary>
    [Input]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsRequired { get; set; }
}