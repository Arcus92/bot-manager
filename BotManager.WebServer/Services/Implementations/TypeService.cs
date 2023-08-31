using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using BotManager.Runtime;
using BotManager.Runtime.Documentations;
using BotManager.WebServer.Models;
using TypeInfo = BotManager.WebServer.Models.TypeInfo;

namespace BotManager.WebServer.Services.Implementations;

public class TypeService : ITypeService
{
    public TypeService()
    {
        RegisterFromExpressions();
    }
    
    #region Documentation

    /// <summary>
    /// The cached assembly documentations.
    /// </summary>
    private readonly Dictionary<Assembly, AssemblyDocumentation> _assemblyDocs = new();
    
    /// <summary>
    /// Gets the documentation for the given assembly. The documentation is cached.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="documentation">The returned documentation.</param>
    /// <returns>Returns <c>true</c> if the documentation xml was found.</returns>
    private bool TryGetAssemblyDocumentation(Assembly assembly, [MaybeNullWhen(false)] out AssemblyDocumentation documentation)
    {
        if (_assemblyDocs.TryGetValue(assembly, out documentation))
            return true;

        var assemblyDocPath = AssemblyDocumentation.GetPathByAssembly(assembly);
        if (!File.Exists(assemblyDocPath)) 
            return false;
        
        documentation = AssemblyDocumentation.Read(assemblyDocPath);
        _assemblyDocs.Add(assembly, documentation);
        return true;
    }

    /// <summary>
    /// Returns the documentation string for the given member.
    /// </summary>
    /// <param name="assembly">The assembly of the member.</param>
    /// <param name="type">The member type.</param>
    /// <param name="name">The member name.</param>
    /// <returns>The raw xml documentation.</returns>
    private string? GetMemberDocumentation(Assembly assembly, AssemblyMemberType type, string name)
    {
        if (!TryGetAssemblyDocumentation(assembly, out var doc))
            return null;

        if (!doc.TryGetMember(type, name, out var member))
            return null;
        
        return member.XmlContent; 
    }

    #endregion Documentation
    
    #region Register

    /// <summary>
    /// The type register map.
    /// </summary>
    private readonly Dictionary<Type, TypeInfo> _typeMap = new();

    /// <inheritdoc />
    public IEnumerable<TypeInfo> Types => _typeMap.Values;
    
    /// <summary>
    /// Registers the given type.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <returns>Returns the created type info.</returns>
    private TypeInfo Register(Type type)
    {
        // Flatting array types
        if (type.IsArray)
        {
            type = type.GetElementType() ?? throw new InvalidOperationException();
        }
        
        // Check existing type.
        if (_typeMap.TryGetValue(type, out var typeInfo))
        {
            return typeInfo;
        }

        typeInfo = CreateTypeInfo(type);
        _typeMap.Add(type, typeInfo);

        // Also register all property types.
        foreach (var propertyInfo in typeInfo.Properties)
        {
            Register(propertyInfo.Type);
        }
        
        return typeInfo;
    }
    
    /// <summary>
    /// Registers all types from the <see cref="IExpression"/>.
    /// </summary>
    private void RegisterFromExpressions()
    {
        foreach (var type in IExpression.Types)
        {
            Register(type);
        }
    }

    /// <summary>
    /// Creates a type info from the given type.
    /// </summary>
    /// <param name="type">The type to convert.</param>
    /// <returns></returns>
    private TypeInfo CreateTypeInfo(Type type)
    {
        var result = new TypeInfo
        {
            TypeName = type.FullName,
            DocumentationXml = GetMemberDocumentation(type.Assembly, AssemblyMemberType.Type, type.FullName ?? ""),
            Properties = GetPropertiesFromType(type),
            IsAbstract = type.IsAbstract
        };

        // Checks if this is a list.
        if (type.IsAssignableTo(typeof(IList)))
            result.IsList = true;

        // Checks if this is a native type.
        if (type == typeof(string) ||
            type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
            type == typeof(bool))
            result.IsNative = true;
        
        // Handle the expression types
        var expressionType = typeof(IExpression);
        if (type != expressionType && type.IsAssignableTo(expressionType))
        {
            // Make sure the expression type is registered.
            Register(expressionType);
            var expressionTypeName = typeof(IExpression).FullName;
            if (expressionTypeName is null) throw new ArgumentException("IExpression FullName is null!");
            result.ParentTypeNames = new[] { expressionTypeName };
            result.ExpressionName = $"${type.Name}";
        }

        // Handle enums
        if (type.IsEnum)
        {
            result.IsEnum = true;
            result.Values = Enum.GetNames(type);
        }

        result.IsObject = result is { IsNative: false, IsEnum: false };
        
        return result;
    }
    
    /// <summary>
    /// Creates a type property info from the given property.
    /// </summary>
    /// <param name="propertyInfo">The property to convert.</param>
    /// <returns></returns>
    private TypePropertyInfo CreateTypePropertyInfo(PropertyInfo propertyInfo)
    {
        var result = new TypePropertyInfo();
        var parentType = propertyInfo.DeclaringType;
        var originalType = propertyInfo.PropertyType;
        result.Type = GetUnderlyingPropertyType(originalType);
        result.Name = propertyInfo.Name;
        result.TypeName = result.Type.FullName;
        
        result.DocumentationXml = GetMemberDocumentation(propertyInfo.Module.Assembly, AssemblyMemberType.Property,
            $"{parentType?.FullName}.{propertyInfo.Name}");
        
        // Checks if this is a root property.
        result.IsRootProperty = propertyInfo.GetCustomAttribute<JsonRootPropertyAttribute>() is not null;

        return result;
    }

    #endregion Register
    
    #region Static
    
    /// <summary>
    /// Returns the raw type of the property, by removing nullable wrapper types.
    /// </summary>
    /// <param name="type">The original type.</param>
    /// <returns></returns>
    private static Type GetUnderlyingPropertyType(Type type)
    {
        // Ignore nullable types.
        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType is not null)
        {
            return GetUnderlyingPropertyType(nullableType);
        }

        return type;
    }
    
    /// <summary>
    /// Returns all input parameters for the given type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    private TypePropertyInfo[] GetPropertiesFromType(Type type)
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
            
            list.Add(CreateTypePropertyInfo(property));
        }
        
        return list.ToArray();
    }
    
    #endregion Static
}