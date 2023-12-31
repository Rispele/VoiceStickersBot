﻿using VoiceStickersBot.Core.CommandArguments;
using VoiceStickersBot.Core.CommandArguments.DeletePackCommandArguments;
using VoiceStickersBot.Core.CommandResults;
using VoiceStickersBot.Core.CommandResults.DeletePackResults;

namespace VoiceStickersBot.Core.CommandHandlers.CommandHandlers.DeletePackHandlers;

public class DeletePackConfirmHandler : ICommandHandler
{
    public CommandType CommandType => CommandType.DeletePack;

    private readonly DeletePackConfirmArguments commandArguments;

    public DeletePackConfirmHandler(DeletePackConfirmArguments commandArguments)
    {
        this.commandArguments = commandArguments;
    }

    public async Task<ICommandResult> Handle()
    {
        var line = new List<InlineKeyboardButtonDto>() 
        { 
            new ("Назад", "DP:SwKbdPc:0:Increase:10"),
            new ("Удалить", $"DP:DeletePack:{commandArguments.StickerPackId}")
        };
        var keyboardDto = new InlineKeyboardDto(new List<List<InlineKeyboardButtonDto>>(), line);
        return new DeletePackConfirmResult(
            commandArguments.ChatId,
            keyboardDto,
            commandArguments.BotMessageId);
    }
}