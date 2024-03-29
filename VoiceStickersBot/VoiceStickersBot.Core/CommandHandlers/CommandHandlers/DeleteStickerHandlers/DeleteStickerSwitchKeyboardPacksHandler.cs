﻿using VoiceStickersBot.Core.CommandArguments;
using VoiceStickersBot.Core.CommandArguments.DeleteStickerCommandArguments;
using VoiceStickersBot.Core.CommandResults;
using VoiceStickersBot.Core.CommandResults.DeleteStickerResults;
using VoiceStickersBot.Core.Repositories.UsersRepository;

namespace VoiceStickersBot.Core.CommandHandlers.CommandHandlers.DeleteStickerHandlers;

public class DeleteStickerSwitchKeyboardPacksHandler : ICommandHandler
{
    public CommandType CommandType => CommandType.DeleteSticker;

    private readonly DeleteStickerSwitchKeyboardPacksArguments commandArguments;
    private readonly IUsersRepository usersRepository;

    public DeleteStickerSwitchKeyboardPacksHandler(
        DeleteStickerSwitchKeyboardPacksArguments commandArguments,
        IUsersRepository usersRepository)
    {
        this.commandArguments = commandArguments;
        this.usersRepository = usersRepository;
    }

    public async Task<ICommandResult> Handle()
    {
        var chatId = commandArguments.ChatId;

        var packs = await usersRepository
            .GetStickerPacksOwned(chatId.ToString(), false)
            .ConfigureAwait(false);

        var hasPacks = packs.Count != 0;
        if (!hasPacks)
            return new DeleteStickerSwitchKeyboardPacksResult(
                chatId,
                new InlineKeyboardDto(
                    new List<List<InlineKeyboardButtonDto>>(),
                    new List<InlineKeyboardButtonDto>()),
                hasPacks,
                commandArguments.BotMessageId);

        var pageFrom = commandArguments.PageFrom;
        var pageTo = commandArguments.Direction == PageChangeDirection.Increase ? pageFrom + 1 : pageFrom - 1;
        var countOnPage = commandArguments.PacksOnPage;

        var buttons = SwitchKeyboardExtensions.BuildMainKeyboardPacks(
            "DS:SwKbdSt",
            ":0:Increase:10",
            packs,
            pageFrom,
            pageTo,
            countOnPage);

        var lastLineButtons = SwitchKeyboardExtensions.BuildLastLine(
            "DS:SwKbdPc",
            "",
            pageTo,
            countOnPage,
            packs!.Count);

        var keyboard = new InlineKeyboardDto(buttons, lastLineButtons);

        return new DeleteStickerSwitchKeyboardPacksResult(chatId, keyboard, hasPacks, commandArguments.BotMessageId);
    }
}