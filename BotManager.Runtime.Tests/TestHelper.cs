namespace BotManager.Runtime.Tests;

/// <summary>
/// Helper class for tests.
/// </summary>
[TestClass]
public static class TestHelper
{
    [AssemblyInitialize]
    public static void Init(TestContext context)
    {
        RuntimePlugin.Register();
    }
    
    /// <summary>
    /// Runs the given json expression.
    /// </summary>
    /// <param name="json">The json string to parse and execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static Task<object?> RunAsync(string json)
    {
        var context = new RuntimeContext();
        return RunAsync(context, json);
    }
    
    /// <summary>
    /// Runs the given expression.
    /// </summary>
    /// <param name="expression">The expression to execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static async Task<object?> RunAsync(IExpression expression)
    {
        var context = new RuntimeContext();
        return await context.ExecuteAsync<object?>(expression);
    }

    /// <summary>
    /// Runs the given json expression.
    /// </summary>
    /// <param name="context">The runtime context to use.</param>
    /// <param name="json">The json string to parse and execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static async Task<object?> RunAsync(RuntimeContext context, string json)
    {
        var expression = await DeserializeExpressionAsync(json);
        return await RunAsync(context, expression);
    }
    
    /// <summary>
    /// Runs the given expression.
    /// </summary>
    /// <param name="context">The runtime context to use.</param>
    /// <param name="expression">The expression to execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static async Task<object?> RunAsync(RuntimeContext context, IExpression expression)
    {
        return await context.ExecuteAsync<object?>(expression);
    }

    /// <summary>
    /// Deserialized the given json expression.
    /// </summary>
    /// <param name="json">The json string to parse.</param>
    /// <returns>Returns the deserialized expression.</returns>
    public static async Task<IExpression> DeserializeExpressionAsync(string json)
    {
        // Copy the input json string into a stream
        using var stream = new MemoryStream();
        await using var jsonStream = new StreamWriter(stream);
        await jsonStream.WriteAsync(json);
        await jsonStream.FlushAsync();
        stream.Position = 0;
   
        // Deserialize the expression and run it
        var expression = IExpression.Deserialize(stream);
        Assert.IsNotNull(expression);
        return expression;
    }
}