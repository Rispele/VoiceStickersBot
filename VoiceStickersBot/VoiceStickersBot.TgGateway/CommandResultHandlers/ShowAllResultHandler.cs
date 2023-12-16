﻿using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceStickersBot.Core.Commands;
using VoiceStickersBot.Core.Commands.ShowAll;
using VoiceStickersBot.Core.Commands.SwitchKeyboard;

namespace VoiceStickersBot.TgGateway.CommandResultHandlers;

public class ShowAllResultHandler : CommandResultHandlerBase<ShowAllResult>
{
    public override Type ResultType => typeof(ShowAllResult);
    public override Task HandleWithCallback(ITelegramBotClient bot, ShowAllResult commandResult, CallbackQuery callbackQuery)
    {
        throw new NotSupportedException();
    }

    public override async Task HandleWithMessage(ITelegramBotClient bot, ShowAllResult commandResult, Message message)
    {
        var abstractCommandResult = (ICommandResult)commandResult;
        var switchKeyboardResult = (SwitchKeyboardResult)abstractCommandResult;
        
        var handler = new SwitchKeyboardResultHandler();
        await handler.HandleWithMessage(bot, switchKeyboardResult, message);
    }
}