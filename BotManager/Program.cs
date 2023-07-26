using System.Text.Json;
using BotManager.Discord;
using BotManager.OpenAi;
using BotManager.Runtime;
using BotManager.Runtime.Converters;

const string tag = "Main";

var context = new RuntimeContext();

if (args.Length == 0)
{
    context.Logger.Error(tag, "No json files provided via command line arguments!");
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
    context.Logger.Error(tag, $"Reading and executing '{configPath}'...");
    
    await using var stream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
    var config = JsonSerializer.Deserialize<IExpression>(stream, jsonOptions);

    // Run
    await context.ExecuteAsync(config);
}

context.Logger.Error(tag, "Execution finished. Waiting for exit...");

await Task.Delay(-1);
return 0;