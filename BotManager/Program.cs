using BotManager.Discord;
using BotManager.OpenAi;
using BotManager.Runtime;

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

// Load each argument as json expression file
foreach (var configPath in args)
{
    context.Logger.Error(tag, $"Reading and executing '{configPath}'...");
    context.RootPath = Path.GetDirectoryName(configPath);
    
    var config = IExpression.Deserialize(configPath);

    // Run
    await context.ExecuteAsync(config);
}

context.Logger.Error(tag, "Execution finished. Waiting for exit...");

await Task.Delay(-1);
return 0;