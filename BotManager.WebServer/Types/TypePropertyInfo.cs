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
    /// <param name="attribute">The input attribute.</param>
    public TypePropertyInfo(PropertyInfo propertyInfo, InputAttribute attribute)
    {
        _propertyInfo = propertyInfo;
        var originalType = _propertyInfo.PropertyType;
        Type = GetRawType(originalType);
        Name = _propertyInfo.Name;
        TypeName = Type.FullName;

        // Check if this is a content property.
        if (attribute.ContentProperty)
            IsContentProperty = true;

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
    public bool IsContentProperty { get; }
    
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
        var list = new List<TypePropertyInfo>();

        // Collects all properties with the expression property attribute attribute
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<InputAttribute>();
            if (attribute is null)
                continue;
            
            list.Add(new TypePropertyInfo(property, attribute));
        }
        
        return list.ToArray();
    }
    
    #endregion Static
}