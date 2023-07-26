namespace BotManager.Runtime.Tests;

[TestClass]
public class NotTests
{
    [TestMethod]
    public async Task TestNotIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Not": false} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestNotIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Not": true} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
}