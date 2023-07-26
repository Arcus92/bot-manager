namespace BotManager.Runtime.Tests;

[TestClass]
public class UInt32Tests
{
    [TestMethod]
    public async Task TestUInt32()
    {
        var result = await TestHelper.RunAsync(""" {"$UInt32": 10} """);
        Assert.IsInstanceOfType(result, typeof(UInt32));
        Assert.AreEqual((UInt32)10, result);
    }
}