﻿using VoiceStickersBot.Core.Commands;
using VoiceStickersBot.Core.Commands.EnterCommand;
using VoiceStickersBot.Core.Commands.SwitchKeyboard;

namespace VoiceStickersBot.Core.CommandHandlers.CommandHandlers;

public class EnterCommandHandler : ICommandHandler
{
    public Type CommandType => typeof(EnterCommand);
    private readonly ICommand command;

    public EnterCommandHandler(ICommand command)
    {
        this.command = command;
    }

    public ICommandResult Handle()
    {
        var botMessageText = "";
        if (command.RequestContext.CommandText is "/show_all" or "Показать все")
            botMessageText = "Выберите стикер и я его отправлю";
        
        var switchCommand = new SwitchKeyboardCommand(command.RequestContext, 0,
            "pageright:1", 10, botMessageText);
        var switchHandler = new SwitchKeyboardHandler(switchCommand);
        return new EnterResult(command.RequestContext.UserBotState, (SwitchKeyboardResult)switchHandler.Handle());
    }
}