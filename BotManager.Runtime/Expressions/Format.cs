namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Formats a string by using <see cref="string.Format(string,object?[])"/>.
/// </para>
/// <para>
/// Return type is <see cref="string"/>.
/// </para>
/// <example>
/// This json example returns <c>"Hello World! 1 + 2 = 3"</c>:
/// <code>
/// { "$Format": { "Text": "Hello {0}! {1} + {2} = {3}", "Parameters": [ "World", 1, 2, 3 ] } }
/// </code>
/// </example>
/// </summary>
public sealed class Format : IExpression
{
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Format()
    {
    }
    
    /// <summary>
    /// Creates am expression that returns the result of <see cref="string.Format(string,object?[])"/>.
    /// </summary>
    /// <param name="text">The format text.</param>
    /// <param name="parameters">The list of parameters to insert.</param>
    public Format(IExpression text, List parameters)
    {
        Text = text;
        Parameters = parameters;
    }

    /// <summary>
    /// Creates am expression that returns the result of <see cref="string.Format(string,object?[])"/>.
    /// </summary>
    /// <param name="text">The format text.</param>
    /// <param name="parameters">The list of parameters to insert.</param>
    public Format(string text, List parameters) : this(new String(text), parameters)
    {
    }

    /// <summary>
    /// Gets and sets the format text
    /// </summary>
    public IExpression? Text { get; set; }

    /// <summary>
    /// Gets and sets the parameter list
    /// </summary>
    public List? Parameters { get; set; }
    

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Text is null)
            return null;

        var text = await context.ExecuteAsync<string>(Text);
        if (text is null)
            return null;

        if (Parameters is null || Parameters.Count == 0)
            return text;

        var arr = await context.ExecuteAsync<object?[]>(Parameters);
        if (arr is null)
            return text;
        
        return string.Format(text, arr);
    }
}