using System.Text.Json.Serialization;

namespace BotManager.WebServer.Models;

/// <summary>
/// This class defines a input property of a <see cref="TypeInfo"/>.
/// </summary>
[Serializable]
public class TypePropertyInfo
{
    /// <summary>
    /// Gets the original type.
    /// </summary>
    [JsonIgnore]
    public Type Type { get; set; } = typeof(void);

    /// <summary>
    /// Gets the name of the expression parameter type.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets the name of the expression parameter type.
    /// </summary>
    public string? TypeName { get; set; }
    
    /// <summary>
    /// Gets the documentation for this property.
    /// </summary>
    public string? DocumentationXml { get; set; }
    
    /// <summary>
    /// The property is written directly to the parent JSON object.
    /// This means there is no property map. The parent object can only have one property.
    /// </summary>
    public bool IsRootProperty { get; set; }
}