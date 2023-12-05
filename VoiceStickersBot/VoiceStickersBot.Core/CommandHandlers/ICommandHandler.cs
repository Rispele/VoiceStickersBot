﻿using VoiceStickersBot.Core.Commands;

namespace VoiceStickersBot.Core.CommandHandlers;

public interface ICommandHandler
{
    Type CommandType { get; }

    ICommandResult Handle();
}