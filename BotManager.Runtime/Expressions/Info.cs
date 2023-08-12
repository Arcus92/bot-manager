using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Writes a log message to the standard output.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
[JsonConverter(typeof(InfoConverter))]
public class Info : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Info()
    {
    }
    
    /// <summary>
    /// Creates an expression that logs the given <paramref name="message"/> in the standard output.
    /// </summary>
    /// <param name="message">The expression to resolve the log message.</param>
    public Info(IExpression? message)
    {
        Message = message;
    }
    
    /// <summary>
    /// Gets and sets the expression to resolve the message to log as information.
    /// </summary>
    [JsonRootProperty]
    public IExpression? Message { get; set; }

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        var message = await context.ExecuteAsync<string>(Message);
        if (message is null)
            return null;
        
        context.Logger.Info("Runtime", message);

        return null;
    }
}