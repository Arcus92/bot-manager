using BotManager.Discord;
using BotManager.OpenAi;
using BotManager.Runtime;
using BotManager.WebServer.Types;

var builder = WebApplication.CreateBuilder(args);

// Register plugins
RuntimePlugin.Register();
DiscordPlugin.Register();
OpenAiPlugin.Register();

TypeInfo.RegisterFromExpressions();

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();