using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;
using BotManager.Runtime;

namespace BotManager.WebServer.Types;

/// <summary>
/// This class defines a input property of a <see cref="TypeInfo"/>.
/// </summary>
[Serializable]
public class TypePropertyInfo
{
    /// <summary>
    /// Creates the expression parameter info by the given property info.
    /// </summary>
    /// <param name="propertyInfo">The property info.</param>
    public TypePropertyInfo(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
        var originalType = _propertyInfo.PropertyType;
        Type = GetRawType(originalType);
        Name = _propertyInfo.Name;
        TypeName = Type.FullName;
        
        // Checks if this is a root property.
        IsRootProperty = propertyInfo.GetCustomAttribute<JsonRootPropertyAttribute>() is not null;
        
        // Checks if this is an array.
        IsArray = originalType.IsArray;
    }

    /// <summary>
    /// The property info.
    /// </summary>
    private readonly PropertyInfo _propertyInfo;

    /// <summary>
    /// Gets the expression parameter type.
    /// </summary>
    [JsonIgnore]
    public Type Type { get; }

    /// <summary>
    /// Gets the name of the expression parameter type.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the name of the expression parameter type.
    /// </summary>
    public string? TypeName { get; }
    
    /// <summary>
    /// The property is written directly to the parent JSON object.
    /// This means there is no property map. The parent object can only have one property.
    /// </summary>
    public bool IsRootProperty { get; }
    
    /// <summary>
    /// Gets if the property is an array.
    /// </summary>
    public bool IsArray { get; }

    #region Static

    /// <summary>
    /// Returns a 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Type GetRawType(Type type)
    {
        // Don't add arrays directly.
        if (type.IsArray)
        {
            return GetRawType(type.GetElementType() ?? throw new InvalidOperationException());
        }
        
        // Ignore nullable types.
        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType is not null)
        {
            return GetRawType(nullableType);
        }

        return type;
    }
    
    /// <summary>
    /// Returns all input parameters for the given type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static TypePropertyInfo[] GetFromType(Type type)
    {
        // Lists don't have any properties.
        if (type.IsAssignableTo(typeof(IList)))
            return Array.Empty<TypePropertyInfo>();
        
        // Collects all properties with the expression property attribute attribute
        var list = new List<TypePropertyInfo>();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var jsonIgnore = property.GetCustomAttribute<JsonIgnoreAttribute>();
            if (jsonIgnore is not null && jsonIgnore.Condition == JsonIgnoreCondition.Always)
                continue;
            
            list.Add(new TypePropertyInfo(property));
        }
        
        return list.ToArray();
    }
    
    #endregion Static
}