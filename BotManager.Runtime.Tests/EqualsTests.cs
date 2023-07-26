namespace BotManager.Runtime.Tests;

[TestClass]
public class EqualsTests
{
    [TestMethod]
    public async Task TestEqualsIsTrue()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": 10, "B": 10}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task TestEqualsIsFalse()
    {
        var result = await TestHelper.RunAsync(""" {"$Equals": {"A": 10, "B": 1}} """);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
}