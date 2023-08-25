using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Returns the value of a variable from the system environment variables.
/// </para>
/// <para>
/// Return type is <see cref="string"/>.
/// </para>
/// <example>
/// This json example returns the value of the environment variable "MY_VAR":
/// <code>
/// { "$Env": "MY_VAR" }
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(EnvConverter))]
[Serializable]
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
    [JsonRootProperty]
    public string Name { get; set; }

    /// <inheritdoc />
    public Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        return Task.FromResult<object?>(Environment.GetEnvironmentVariable(Name));
    }
}