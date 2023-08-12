using System.Text.Json.Serialization;
using BotManager.Runtime;
using BotManager.Runtime.Converters;
using Discord;
using Discord.WebSocket;

namespace BotManager.Discord.Expressions;

/// <summary>
/// The Discord plugin will connect to the Discord api using the given <see cref="Token"/>.
/// You can register <see cref="Commands"/> that will run an expression once a user executes them.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
public sealed class DiscordInit : IExpression
{
    internal const string Tag = "Discord";

    /// <summary>
    /// The operation context the plugin was initialized with
    /// </summary>
    private RuntimeContext _context = new();
    
    #region Config

    /// <summary>
    /// Gets and sets the <see cref="Token"/> type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TokenType>))]
    public TokenType TokenType { get; set; } = TokenType.Bot;

    /// <summary>
    /// Gets and sets the Discord API key
    /// </summary>
    public IExpression? Token { get; set; }

    /// <summary>
    /// Gets and sets the slash commands
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DiscordSlashCommand[]? Commands { get; set; }

    #endregion Config
    
    /// <summary>
    /// The Discord client
    /// </summary>
    private readonly DiscordSocketClient _client = new();

    /// <summary>
    /// Gets the Discord client interface.
    /// </summary>
    [JsonIgnore]
    public DiscordSocketClient Client => _client;
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        _context = context;
        context.Logger.Info(Tag, "Initialize Discord plugin...");
        
        // Read the token
        var token = await context.ExecuteAsync<string>(Token);
        if (token is null)
            throw new ArgumentException("Token is null.");
        
        _client.Log += OnLog;
        _client.Ready += OnReady;
        _client.SlashCommandExecuted += OnSlashCommandExecuted;

        // Init
        await _client.LoginAsync(TokenType, token);
        await _client.StartAsync();

        // Guilds are still not loaded here. We need to wait for OnClientReady.
        
        
        // Register the plugin
        context.Set("discord.plugin", this);
        
        return null;
    }

    /// <summary>
    /// Discord logs a message
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    private Task OnLog(LogMessage arg)
    {
        switch (arg.Severity)
        {
            case LogSeverity.Critical:
            case LogSeverity.Error:
                _context.Logger.Error(Tag, arg.Message);
                break;
            default:
                _context.Logger.Info(Tag, arg.Message);
                break;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Discord is ready and all guilds are loaded
    /// </summary>
    /// <returns></returns>
    private async Task OnReady()
    {
        _context.Logger.Info(Tag, "Discord is ready!");
        
        await RegisterSlashCommandsAsync();
    }
    
    #region Helper
    
    /// <summary>
    /// Returns the guild by the given identifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public IGuild? GetGuild(DiscordIdentifier identifier)
    {
        if (identifier.Name is not null)
            return _client.Guilds.FirstOrDefault(g => g.Name == identifier.Name);
        return _client.GetGuild(identifier.Id);
    }

    /// <summary>
    /// Returns the guild by the given identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IGuild? GetGuild(ulong id)
    {
        return _client.GetGuild(id);
    }
    
    #endregion Helper
    
    #region Slash command

    /// <summary>
    /// Registers the slash commands
    /// </summary>
    private async Task RegisterSlashCommandsAsync()
    {
        if (Commands is null)
            return;
        
        foreach (var command in Commands)
        {
            // Ignore empty name
            if (command.Name is null)
            {
                _context.Logger.Error(Tag, "Skipping slash command without name.");
                continue;
            }

            try
            {
                var builder = new SlashCommandBuilder();
                builder.WithName(command.Name);
                builder.WithDescription(command.Description);

                // Register the parameters
                if (command.Options is not null)
                {
                    foreach (var option in command.Options)
                    {
                        builder.AddOption(option.Name, option.Type, option.Description, option.IsRequired);
                    }
                }

                var build = builder.Build();
                if (command.Guild.HasValue)
                {
                    _context.Logger.Info(Tag, $"Register slash command '{command.Name}' in '{command.Guild}'...");
                    
                    // Guild commands
                    var guild = GetGuild(command.Guild.Value);
                    if (guild is not null)
                    {
                        await guild.CreateApplicationCommandAsync(build);
                    }
                    else
                    {
                        _context.Logger.Error(Tag, $"Could not find guild '{command.Guild}'. Skipping slash command...");
                        continue;
                    }
                }
                else
                {
                    _context.Logger.Info(Tag, $"Register global slash command '{command.Name}'...");
                    
                    // Global commands
                    await _client.CreateGlobalApplicationCommandAsync(build);
                }
                
                _context.Logger.Info(Tag, $"Slash command '{command.Name}' was successfully registered.");
            }
            catch (Exception ex)
            {
                _context.Logger.Error(Tag, $"Failed to register slash command '{command.Name}': " + ex);
            }
        }
    }

    /// <summary>
    /// A slash command was executed
    /// </summary>
    /// <param name="slashCommand"></param>
    private async Task OnSlashCommandExecuted(SocketSlashCommand slashCommand)
    {
        if (Commands is null)
            return;

        // We don't want to react to bot commands, right?
        if (slashCommand.User.IsBot)
            return;

        // Try to find the command
        var command = Commands.FirstOrDefault(c => c.Name == slashCommand.CommandName);
        if (command is null)
            return;
        
        _context.Logger.Info(Tag, $"Handle slash command '{command.Name}...'");

        // Clones the context
        var context = new RuntimeContext(_context);
        
        // Register all the options
        context.Set("discord.command", slashCommand);
        foreach (var option in slashCommand.Data.Options)
        {
            context.Set($"discord.command.options.{option.Name}", option.Value);
        }
        
        // Sets the guild
        if (slashCommand.GuildId.HasValue)
        {
            var guild = GetGuild(slashCommand.GuildId.Value);
            if (guild is not null)
            {
                context.Set("discord.guild", guild);
                context.Set("discord.guild.id", slashCommand.GuildId.Value);
                context.Set("discord.guild.name", guild.Name);
            }
        }

        // Sets the channel
        if (slashCommand.Channel is not null)
        {
            context.Set("discord.channel", slashCommand.Channel);
            context.Set("discord.channel.id", slashCommand.Channel.Id);
            context.Set("discord.channel.name", slashCommand.Channel.Name);
        }

        // Sets the user
        context.Set("discord.user", slashCommand.User);
        context.Set("discord.user.id", slashCommand.User.Id);
        context.Set("discord.user.name", slashCommand.User.Username);

        try
        {
            await context.ExecuteAsync(command.Action);
        }
        catch (Exception ex)
        {
            _context.Logger.Error(Tag, $"Exception while executing slash command '{command.Name}': " + ex);
        }
        
    }
    
    #endregion Slash command
}