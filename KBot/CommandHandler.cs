using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;

namespace KBot;

public class CommandHandler : DiscordClientService
{
    private readonly IServiceProvider _provider;
    private readonly CommandService _commandService;
    private readonly IConfiguration _config;
    
    public CommandHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider, CommandService commandService, IConfiguration config) : base(client, logger)
    {
        _provider = provider;
        _commandService = commandService;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.MessageReceived += HandleMessage;
        _commandService.CommandExecuted += CommandExecutedAsync;

        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
    }

    private async Task HandleMessage(SocketMessage incomingMessage)
    {
        if (incomingMessage is not SocketUserMessage message) return;
        if (message.Source != MessageSource.User) return;

        int argPos = 0;
        
        //TODO: move into config at some point soon and then into DB with config per 'guild' in the future.
        if (!message.HasStringPrefix("!", ref argPos) && !message.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

        var context = new SocketCommandContext(Client, message);
        await _commandService.ExecuteAsync(context, argPos, _provider);
    }
    
    private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        Logger.LogInformation("User {user} attempted to use the command {command}", context.User, command.Value.Name);

        if (!command.IsSpecified || result.IsSuccess)
            return;

        await context.Channel.SendMessageAsync($"Error: {result}");
    }
}