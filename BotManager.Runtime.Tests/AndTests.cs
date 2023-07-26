namespace BotManager.Runtime.Tests;

[TestClass]
public class AndTests
{
    [TestMethod]
    public async Task TestAndIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$And": [true, true]} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
    
    [TestMethod]
    public async Task TestAndIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$And": [true, false]} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestEmptyAndIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$And": []} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
}