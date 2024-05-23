using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using KBot;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDiscordHost((config, _) =>
{
    config.SocketConfig = new DiscordSocketConfig
    {
        LogLevel = LogSeverity.Verbose,
        AlwaysDownloadUsers = true, 
        MessageCacheSize = 200,
        GatewayIntents = GatewayIntents.All
    };

    config.Token = builder.Configuration["token"] ?? string.Empty;
});

builder.Services.AddCommandService((config, _) =>
{
    config.DefaultRunMode = RunMode.Async;
    config.CaseSensitiveCommands = false;
});

builder.Services.AddInteractionService((config, _) =>
{
    config.LogLevel = LogSeverity.Info;
    config.UseCompiledLambda = true;
});




builder.Services.AddHostedService<KBotHeart>();

var host = builder.Build();
await host.RunAsync();