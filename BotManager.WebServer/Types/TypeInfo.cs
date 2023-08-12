using System.Collections;
using System.Text.Json.Serialization;
using BotManager.Runtime;

namespace BotManager.WebServer.Types;

/// <summary>
/// The type info is used to translate the C# types to the JavaScript world.
/// Most types are <see cref="IExpression"/>s but it also supports other custom classes.
/// </summary>
[Serializable]
public class TypeInfo
{
    /// <summary>
    /// Creates the expression list.
    /// </summary>
    /// <param name="type"></param>
    public TypeInfo(Type type)
    {
        Type = type;
        TypeName = type.FullName;
        Properties = TypePropertyInfo.GetFromType(type);

        IsAbstract = type.IsAbstract;
        
        // Checks if this is a list.
        if (type.IsAssignableTo(typeof(IList)))
            IsList = true;

        // Checks if this is a native type.
        if (type == typeof(string) ||
            type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
            type == typeof(bool))
            IsNative = true;
        
        // Handle the expression types
        var expressionType = typeof(IExpression);
        if (type != expressionType && type.IsAssignableTo(expressionType))
        {
            // Make sure the expression type is registered.
            Register(expressionType);
            var expressionTypeName = typeof(IExpression).FullName;
            if (expressionTypeName is null) throw new ArgumentException("IExpression FullName is null!");
            ParentTypeNames = new[] { expressionTypeName };
            ExpressionName = $"${type.Name}";
        }

        // Handle enums
        if (type.IsEnum)
        {
            IsEnum = true;
            Values = Enum.GetNames(type);
        }
    }

    /// <summary>
    /// Gets the expression type.
    /// </summary>
    [JsonIgnore]
    private Type Type { get; }

    /// <summary>
    /// Gets the name of the type name.
    /// </summary>
    public string? TypeName { get; }

    /// <summary>
    /// Gets the name of the expression.
    /// </summary>
    public string? ExpressionName { get; }

    /// <summary>
    /// Gets if this type is abstract an can not be instantiated.
    /// </summary>
    public bool IsAbstract { get; }

    /// <summary>
    /// Gets if this is a native type like string, numbers or boolean.
    /// </summary>
    public bool IsNative { get; }

    /// <summary>
    /// Gets if this is a list.
    /// </summary>
    public bool IsList { get; }

    /// <summary>
    /// Gets if this is an enum. Is set, <see cref="Values"/> contains all enum values.
    /// </summary>
    public bool IsEnum { get; }

    /// <summary>
    /// Gets and sets the list of parent types.
    /// </summary>
    public string[] ParentTypeNames { get; } = Array.Empty<string>();
    
    /// <summary>
    /// Gets all available input properties of this expression. 
    /// </summary>
    public TypePropertyInfo[] Properties { get; }

    /// <summary>
    /// Gets all values for an enum type.
    /// </summary>
    public string[] Values { get; } = Array.Empty<string>();

    #region Register

    /// <summary>
    /// The type register map.
    /// </summary>
    private static readonly Dictionary<Type, TypeInfo> TypeMap = new();

    /// <summary>
    /// Gets all registered types.
    /// </summary>
    public static IEnumerable<TypeInfo> Types => TypeMap.Values;
    
    /// <summary>
    /// Registers the given type.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <returns>Returns the created type info.</returns>
    public static TypeInfo Register(Type type)
    {
        // Check existing type.
        if (TypeMap.TryGetValue(type, out var typeInfo))
        {
            return typeInfo;
        }

        typeInfo = new TypeInfo(type);
        TypeMap.Add(type, typeInfo);

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
    public static void RegisterFromExpressions()
    {
        foreach (var type in IExpression.Types)
        {
            Register(type);
        }
    }

    #endregion Register
}