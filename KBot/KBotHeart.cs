using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;

namespace KBot;

public class KBotHeart(DiscordSocketClient client, ILogger<DiscordClientService> logger)
    : DiscordClientService(client, logger)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Client.WaitForReadyAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.LogInformation("Still here at {time}", DateTimeOffset.UtcNow);
            await Task.Delay(1000, stoppingToken);
        }
    }
}