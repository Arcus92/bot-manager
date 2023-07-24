using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BotManager.Runtime;

/// <summary>
/// The expression interface defines one operation that can be serialized in a json file.
/// Use the <see cref="RuntimeContext"/> to execute these expressions.
/// </summary>
public interface IExpression
{
    /// <summary>
    /// Executes the expression in the given <see cref="RuntimeContext"/>.
    /// Please do not use this method to run an expression. Use <see cref="RuntimeContext.ExecuteAsync(BotManager.Runtime.IExpression?)"/> instead.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <param name="returnType">
    /// The expected return type. Use <c>typeof(object)</c> for a generic result. Use <c>null</c> if you don't need the
    /// return value.
    /// </param>
    /// <returns>Returns the result of the expression.</returns>
    Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType);

    #region Type map
    
    // For fast serialization, we store a static map for the name and the IExpression types.
    // If a type is not registered it will be not deserialized!
    
    /// <summary>
    /// The internal type map.
    /// </summary>
    private static readonly Dictionary<string, Type> ExpressionTypeMap = new();

    /// <summary>
    /// Registers a new type for the serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void RegisterExpressionType<T>() where T : IExpression
    {
        RegisterExpressionType(typeof(T));
    }
    
    /// <summary>
    /// Registers a new type for the serializer.
    /// </summary>
    /// <param name="type"></param>
    public static void RegisterExpressionType(Type type) 
    {
        ExpressionTypeMap.Add(type.Name, type);
    }
    
    /// <summary>
    /// Registers all public <see cref="IExpression"/> types from the serializer in the given assembly.
    /// </summary>
    /// <param name="assembly"></param>
    public static void RegisterExpressionTypesFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.ExportedTypes)
        {
            if (type.IsAbstract)
                continue;
            
            if (type.IsAssignableTo(typeof(IExpression)))
                RegisterExpressionType(type);
        }
    }

    /// <summary>
    /// Tries to gets a registered type by it's name.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <param name="type">The returned type.</param>
    /// <returns>Returns <c>true</c> if a type was found.</returns>
    public static bool TryGetExpressionType(string typeName, [MaybeNullWhen(false)] out Type type)
    {
        return ExpressionTypeMap.TryGetValue(typeName, out type);
    }
    
    #endregion Type map
}