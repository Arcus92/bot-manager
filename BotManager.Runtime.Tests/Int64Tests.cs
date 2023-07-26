namespace BotManager.Runtime.Tests;

[TestClass]
public class Int64Tests
{
    [TestMethod]
    public async Task TestInt64()
    {
        var result = await TestHelper.RunAsync(""" {"$Int64": 10} """);
        Assert.IsInstanceOfType(result, typeof(Int64));
        Assert.AreEqual((Int64)10, result);
    }
}