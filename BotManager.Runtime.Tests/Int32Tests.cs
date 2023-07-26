namespace BotManager.Runtime.Tests;

[TestClass]
public class Int32Tests
{
    [TestMethod]
    public async Task TestInt32()
    {
        var result = await TestHelper.RunAsync(""" {"$Int32": 10} """);
        Assert.IsInstanceOfType(result, typeof(Int32));
        Assert.AreEqual(10, result);
    }
    
    [TestMethod]
    public async Task TestInt32Short()
    {
        var result = await TestHelper.RunAsync(""" 10 """);
        Assert.IsInstanceOfType(result, typeof(Int32));
        Assert.AreEqual(10, result);
    }
}