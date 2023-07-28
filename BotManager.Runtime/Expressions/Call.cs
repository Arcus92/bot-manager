using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Calls a registered function and returns it's value. Use <see cref="Function"/> to register a function beforehand.
/// <para>
/// Returns: Return value of the executed function.
/// </para>
/// </summary>
[JsonConverter(typeof(CallConverter))]
public class Call : IExpression
{
    internal const string Tag = "Call";
    
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Call()
    {
        Name = string.Empty;
    }
    
    /// <summary>
    /// Creates an expression that calls the registered function with the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    public Call(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// Gets and sets the name of the function to call.
    /// </summary>
    public string Name { get; set; }
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (!context.Functions.TryGet(Name, out var function))
        {
            context.Logger.Error(Tag, $"Could not find defined function '{Name}'.");
            return null;
        }

        return await context.ExecuteAsync(function, returnType);
    }
}