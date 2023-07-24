using System.Text.Json;
using BotManager.Discord;
using BotManager.OpenAi;
using BotManager.Runtime;
using BotManager.Runtime.Converters;

// Register plugins
RuntimePlugin.Register();
DiscordPlugin.Register();
OpenAiPlugin.Register();

// Load the config
var jsonOptions = new JsonSerializerOptions
{
    Converters = { new ExpressionConverter() },
};
var stream = new FileStream("run.json", FileMode.Open);
var config = JsonSerializer.Deserialize<IExpression>(stream, jsonOptions);

// Run
var context = new RuntimeContext();
await context.ExecuteAsync(config);

await Task.Delay(-1);