using BotManager.Runtime;

namespace BotManager.WebServer.Models;

/// <summary>
/// The type info is used to translate the C# types to the JavaScript world.
/// Most types are <see cref="IExpression"/>s but it also supports other custom classes.
/// </summary>
[Serializable]
public class TypeInfo
{
    /// <summary>
    /// Gets the name of the type name.
    /// </summary>
    public string? TypeName { get; set; }
    
    /// <summary>
    /// Gets the documentation for this type.
    /// </summary>
    public string? DocumentationXml { get; set; }

    /// <summary>
    /// Gets the name of the expression.
    /// </summary>
    public string? ExpressionName { get; set; }

    /// <summary>
    /// Gets if this type is abstract an can not be instantiated.
    /// </summary>
    public bool IsAbstract { get; set; }

    /// <summary>
    /// Gets if this is a object type.
    /// </summary>
    public bool IsObject { get; set; }
    
    /// <summary>
    /// Gets if this is a native type like string, numbers or boolean.
    /// </summary>
    public bool IsNative { get; set; }

    /// <summary>
    /// Gets if this is a list.
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    /// Gets if this is an enum. Is set, <see cref="Values"/> contains all enum values.
    /// </summary>
    public bool IsEnum { get; set; }

    /// <summary>
    /// Gets and sets the list of parent types.
    /// </summary>
    public string[] ParentTypeNames { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets all available input properties of this expression. 
    /// </summary>
    public TypePropertyInfo[] Properties { get; set; } = Array.Empty<TypePropertyInfo>();

    /// <summary>
    /// Gets all values for an enum type.
    /// </summary>
    public string[] Values { get; set; } = Array.Empty<string>();
}