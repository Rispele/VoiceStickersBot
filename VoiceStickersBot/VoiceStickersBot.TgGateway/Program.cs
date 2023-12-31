﻿using Ninject;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using VoiceStickersBot.Infra.VSBApplication;
using VoiceStickersBot.Infra.VSBApplication.Settings;
using VoiceStickersBot.TgGateway;

using CancellationTokenSource cts = new();
var host = new TgApiGatewayHost();

await host.RunAsync(() => cts.Token);

Console.ReadLine();
cts.Cancel();

[Settings("TgApiGatewaySettings")]
public class TgApiGatewayHost : VsbApplicationBase
{
    protected override async Task RunAsync(CancellationToken cancellationToken)
    {
        var botClient = Container.Get<ITelegramBotClient>();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        var handler = Container.Get<TgApiGatewayService>();
        botClient.StartReceiving(
            handler.HandleUpdateAsync,
            handler.HandlePollingErrorAsync,
            receiverOptions,
            cancellationToken
        );

        var me = await botClient.GetMeAsync(cancellationToken);
        Console.WriteLine($"Start listening for @{me.Username}");
    }

    protected override void ConfigureContainer(StandardKernel containerBuilder)
    {
        containerBuilder
            .BindCommandArgumentsFactories()
            .BindCommandHandlerFactories()
            .BindCommandResultHandlers();

        var botClient = new TelegramBotClient(ApplicationSettings.Settings["token"]);

        containerBuilder.Bind<ITelegramBotClient>().ToConstant(botClient);

        base.ConfigureContainer(containerBuilder);
    }
}