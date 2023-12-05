﻿using VoiceStickersBot.Core.Commands;

namespace VoiceStickersBot.Core.CommandHandlers;

public class MainCommandHandler
{
    private readonly Dictionary<Type, ICommandHandlerFactory> commandHandlers;

    public MainCommandHandler(List<ICommandHandlerFactory> commandHandlers)
    {
        this.commandHandlers = commandHandlers.ToDictionary(
            key => key.CommandType,
            value => value);
    }

    public Type CommandType { get; }

    public ICommandResult Handle(ICommand command)
    {
        var commandHandler = commandHandlers[command.GetType()].CreateCommandHandler(command);
        return commandHandler.Handle();
    }
}