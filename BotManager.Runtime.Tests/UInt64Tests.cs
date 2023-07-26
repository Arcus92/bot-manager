namespace BotManager.Runtime.Tests;

[TestClass]
public class UInt64Tests
{
    [TestMethod]
    public async Task TestUInt64()
    {
        var result = await TestHelper.RunAsync(""" {"$UInt64": 10} """);
        Assert.IsInstanceOfType(result, typeof(UInt64));
        Assert.AreEqual((UInt64)10, result);
    }
}