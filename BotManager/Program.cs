using System.Text.Json;
using BotManager.Discord;
using BotManager.OpenAi;
using BotManager.Runtime;
using BotManager.Runtime.Converters;

if (args.Length == 0)
{
    Console.Error.WriteLine("No json files provided via command line arguments!");
    return -1;
}

// Register plugins
RuntimePlugin.Register();
DiscordPlugin.Register();
OpenAiPlugin.Register();

// Load the config
var jsonOptions = new JsonSerializerOptions
{
    Converters = { new ExpressionConverter() },
};

// Load each argument as json expression file
foreach (var configPath in args)
{
    await using var stream = new FileStream(configPath, FileMode.Open);
    var config = JsonSerializer.Deserialize<IExpression>(stream, jsonOptions);

    // Run
    var context = new RuntimeContext();
    await context.ExecuteAsync(config);
}

await Task.Delay(-1);
return 0;