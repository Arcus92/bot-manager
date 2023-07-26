namespace BotManager.Runtime.Tests;

[TestClass]
public class OrTests
{
    [TestMethod]
    public async Task TestOrIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Or": [true, false]} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
    
    [TestMethod]
    public async Task TestOrIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Or": [false, false]} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestEmptyOrIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Or": []} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
}