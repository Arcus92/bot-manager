namespace BotManager.Runtime.Tests;

[TestClass]
public class EqualsTests
{
    [TestMethod]
    public async Task TestEqualsInt32IsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": 10, "B": 10}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestEqualsInt32IsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": 10, "B": 1}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestEqualsStringIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": "Test", "B": "Test"}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestEqualsStringIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": "Test", "B": "Hello"}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
}