using System.Text.Json.Serialization;
using BotManager.Runtime;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace BotManager.OpenAi.Expressions;

/// <summary>
/// The OpenAI plugin will connect to the OpenAI api by using the <see cref="ApiKey"/> and allows you to run AI
/// expressions like <see cref="OpenAiChat"/>.
/// <para>
/// Returns: <c>null</c>.
/// </para>
/// </summary>
public sealed class OpenAiInit : IExpression
{
    internal const string Tag = "OpenAI";
    
    #region Config
    
    /// <summary>
    /// Gets and sets the OpenAI api key.
    /// </summary>
    public IExpression? ApiKey { get; set; }

    #endregion Config

    /// <summary>
    /// The OpenAI service
    /// </summary>
    private OpenAIService? _service;

    /// <summary>
    /// Gets the internal OpenAI service.
    /// </summary>
    [JsonIgnore]
    public OpenAIService Service => _service ?? throw new InvalidOperationException("OpenAI service is not initialized.");
    
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        context.Logger.Info(Tag, "Initialize OpenAI plugin...");
        
        // Read the api key
        var apiKey = await context.ExecuteAsync<string>(ApiKey);
        if (apiKey is null)
            throw new ArgumentException("ApiKey is null.");

        // Create the service
        _service = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = apiKey
        });

        // Register the plugin
        context.Set("openai.plugin", this);
        
        return null;
    }
}