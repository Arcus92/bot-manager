using BotManager.Runtime;
using Microsoft.AspNetCore.Mvc;

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
        return IExpression.Deserialize("../examples/chef-gpt.json") ?? throw new NullReferenceException();
    }
}