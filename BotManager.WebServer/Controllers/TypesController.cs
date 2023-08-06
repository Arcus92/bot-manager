using BotManager.WebServer.Types;
using Microsoft.AspNetCore.Mvc;

namespace BotManager.WebServer.Controllers;

/// <summary>
/// The type api returns a list of all registered types that can be used in the config builder.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TypesController : ControllerBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<TypesController> _logger;

    public TypesController(ILogger<TypesController> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Returns the complete type list for the configuration.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<TypeInfo> Get()
    {
        return TypeInfo.Types;
    }
}