using BotManager.Discord;
using BotManager.Discord.Expressions;
using BotManager.Runtime;
using BotManager.Runtime.Expressions;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Boolean = BotManager.Runtime.Expressions.Boolean;
using Format = BotManager.Runtime.Expressions.Format;
using Int32 = BotManager.Runtime.Expressions.Int32;
using UInt32 = BotManager.Runtime.Expressions.UInt32;

namespace BotManager.WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    /// <summary>
    /// Returns an example configuration.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IExpression Get()
    {
        return new List
        {
            new DiscordInit()
            {
                Token = new Env("DiscordToken"),
                TokenType = TokenType.Bot,
                Commands = new []
                {
                    new DiscordSlashCommand()
                    {
                        Name = "test",
                        Options = new []
                        {
                            new DiscordSlashCommandOption()
                            {
                                Name = "description",
                                Type = ApplicationCommandOptionType.String,
                                IsRequired = true,
                            }
                        }
                    }
                }
            },
            new Set("MyVar", new Boolean(true)),
            new Info(new Format("Hello World: {0} {1} {2}", new List()
            {
                new Get("MyVar"),
                new Int32(1337),
                new UInt32(1337)
            }))
        };
    }
}