namespace BotManager.Runtime.Tests;

[TestClass]
public class StringTests
{
    [TestMethod]
    public async Task TestString()
    {
        var result = await TestHelper.RunAsync(""" {"$String": "Test"} """);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("Test", result);
    }
    
    [TestMethod]
    public async Task TestStringShort()
    {
        var result = await TestHelper.RunAsync(""" "Test" """);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual("Test", result);
    }
}