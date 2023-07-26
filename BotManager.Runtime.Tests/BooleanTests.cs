namespace BotManager.Runtime.Tests;

[TestClass]
public class BooleanTests
{
    [TestMethod]
    public async Task TestBooleanIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Boolean": true} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
    
    [TestMethod]
    public async Task TestBooleanIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Boolean": false} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestBooleanShortIsTrue()
    {
        var result = await TestHelper.RunAsync(""" true """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
    
    [TestMethod]
    public async Task TestBooleanShortIsFalse()
    {
        var result = await TestHelper.RunAsync(""" false """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
}