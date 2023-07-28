namespace BotManager.Runtime.Tests;

[TestClass]
public class FunctionTests
{
    [TestMethod]
    public async Task TestFunction()
    {
        var result = await TestHelper.RunAsync(""" [{"$Function":{"Name": "FuncTest", "Action": { "$Int32": 10 }}},{"$Call": "FuncTest"}] """);
        Assert.IsInstanceOfType(result, typeof(object[]));
        var array = (object[])result!;
        Assert.AreEqual(2, array.Length);
        Assert.IsInstanceOfType(array[1], typeof(int));
        Assert.AreEqual(10, array[1]);
    }
    
}