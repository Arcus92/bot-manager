namespace BotManager.Runtime.Tests;

[TestClass]
public class InTests
{
    [TestMethod]
    public async Task TestInInt32IsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$In": {"Value": 10, "List": [10, 20]}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestInInt32IsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$In": {"Value": 10, "List": [1, 2]}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestInStringIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$In": {"Value": "Test", "List": ["Hello", "Test"]}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestInStringIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$In": {"Value": "Test", "List": ["Hello", "World"]}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
}