using BotManager.WebServer.Models;

namespace BotManager.WebServer.Services;

public interface ITypeService
{
    /// <summary>
    /// Gets all registered types.
    /// </summary>
    IEnumerable<TypeInfo> Types { get; }
}