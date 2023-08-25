using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Writes a log message to the error output.
/// </para>
/// <para>
/// Return type is <c>null</c>.
/// </para>
/// <example>
/// This json example writes an error to the output and returns <c>null</c>:
/// <code>
/// { "$Error": "This is an error!" }
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(ErrorConverter))]
[Serializable]
public sealed class Error : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Error()
    {
    }
    
    /// <summary>
    /// Creates an expression that logs the given <paramref name="message"/> in the error output.
    /// </summary>
    /// <param name="message">The expression to resolve the log message.</param>
    public Error(IExpression? message)
    {
        Message = message;
    }
    
    /// <summary>
    /// Gets and sets the expression to resolve the message to log as error.
    /// </summary>
    [JsonRootProperty]
    public IExpression? Message { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var message = await context.ExecuteAsync<string>(Message);
        if (message is null)
            return null;
        
        context.Logger.Error("Runtime", message);

        return null;
    }
}