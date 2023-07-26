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
    /// Runs the given json
    /// </summary>
    /// <param name="json">The json string to parse and execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static Task<object?> RunAsync(string json)
    {
        var context = new RuntimeContext();
        return RunAsync(context, json);
    }

    /// <summary>
    /// Runs the given json
    /// </summary>
    /// <param name="context">The runtime context to use.</param>
    /// <param name="json">The json string to parse and execute.</param>
    /// <returns>Returns the return value of the expression.</returns>
    public static async Task<object?> RunAsync(RuntimeContext context, string json)
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
        return await context.ExecuteAsync<object?>(expression);
    }
}