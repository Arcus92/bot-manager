using BotManager.WebServer.Models;
using BotManager.WebServer.Services;
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

    /// <summary>
    /// The type service.
    /// </summary>
    private readonly ITypeService _typeService;

    public TypesController(ILogger<TypesController> logger, ITypeService typeService)
    {
        _logger = logger;
        _typeService = typeService;
    }
    
    /// <summary>
    /// Returns the complete type list for the configuration.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<TypeInfo> Get()
    {
        return _typeService.Types;
    }
}