using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Returns the value of a variable from the system environment variables.
/// <para>
/// Returns: <see cref="string"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(EnvConverter))]
public sealed class Env : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Env()
    {
        Name = string.Empty;
    }
    
    /// <summary>
    /// Creates an expression that returns the system environment variable with the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    public Env(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// Gets and sets the name of the variable
    /// </summary>
    [Input(ContentProperty = true)]
    public string Name { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Environment.GetEnvironmentVariable(Name));
    }
}