using BotManager.Runtime.Utils;

namespace BotManager.Runtime.Tests;

[TestClass]
public class CooldownTests
{
    [TestMethod]
    public async Task TestCooldownIsBlocking()
    {
        // In this test environment, we need to simulate the passing of time.
        var context = new RuntimeContext();
        var dateTimeProvider = new TestingDateTimeProvider();
        context.DateTimeProvider = dateTimeProvider;
        dateTimeProvider.Now = new DateTime(2023, 1, 1, 10, 0, 0);

        // We cannot simply run this, because we need to change our fake time between calls.
        var cooldownExpression =
            await TestHelper.DeserializeExpressionAsync(
                """ {"$Cooldown": {"Action": true, "Fallback": false, "Milliseconds": 300000}} """);

        // Actual test:
        var result = await TestHelper.RunAsync(context, cooldownExpression);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
        
        // Two minutes later
        dateTimeProvider.Now = new DateTime(2023, 1, 1, 10, 2, 0);
        
        result = await TestHelper.RunAsync(context, cooldownExpression);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task TestCooldownIsNotBlocking()
    {
        // In this test environment, we need to simulate the passing of time.
        var context = new RuntimeContext();
        var dateTimeProvider = new TestingDateTimeProvider();
        context.DateTimeProvider = dateTimeProvider;
        dateTimeProvider.Now = new DateTime(2023, 1, 1, 10, 0, 0);

        // We cannot simply run this, because we need to change our fake time between calls.
        var cooldownExpression =
            await TestHelper.DeserializeExpressionAsync(
                """ {"$Cooldown": {"Action": true, "Fallback": false, "Milliseconds": 300000}} """);

        // Actual test:
        var result = await TestHelper.RunAsync(context, cooldownExpression);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
        
        // Ten minutes later
        dateTimeProvider.Now = new DateTime(2023, 1, 1, 10, 10, 0);
        
        result = await TestHelper.RunAsync(context, cooldownExpression);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
}