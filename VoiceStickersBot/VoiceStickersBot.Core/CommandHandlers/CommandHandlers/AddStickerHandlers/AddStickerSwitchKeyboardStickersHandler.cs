﻿using VoiceStickersBot.Core.CommandArguments;
using VoiceStickersBot.Core.CommandArguments.AddStickerCommandArguments;
using VoiceStickersBot.Core.CommandResults;
using VoiceStickersBot.Core.CommandResults.AddStickerResults;
using VoiceStickersBot.Core.Contracts;
using VoiceStickersBot.Core.Repositories.StickerPacksRepository;

namespace VoiceStickersBot.Core.CommandHandlers.CommandHandlers.AddStickerHandlers;

public class AddStickerSwitchKeyboardStickersHandler : ICommandHandler
{
    public CommandType CommandType => CommandType.AddSticker;

    private readonly AddStickerSwitchKeyboardStickersArguments commandArguments;
    private readonly StickerPacksRepository stickerPacksRepository;
    
    public AddStickerSwitchKeyboardStickersHandler(
        AddStickerSwitchKeyboardStickersArguments commandArguments,
        StickerPacksRepository stickerPacksRepository)
    {
        this.commandArguments = commandArguments;
        this.stickerPacksRepository = stickerPacksRepository;
    }

    public async Task<ICommandResult> Handle()
    {
        var stickerPack = await stickerPacksRepository
            .GetStickerPackAsync(commandArguments.StickerPackId, true)
            .ConfigureAwait(false);
        var stickers = stickerPack.Stickers ?? new List<Sticker>();

        var pageFrom = commandArguments.PageFrom;
        var pageTo = commandArguments.Direction == PageChangeDirection.Increase ? pageFrom + 1 : pageFrom - 1;
        var countOnPage = commandArguments.PacksOnPage;

        var callbackPrefix = "AS:SendSticker";
        var buttons = SwitchKeyboardExtensions.BuildMainKeyboardStickers(
            callbackPrefix,
            stickers,
            pageFrom,
            pageTo,
            countOnPage);

        buttons.Add(new InlineKeyboardButtonDto("Назад", $"AS:SwKbdPc:{commandArguments.StickerPackId}:0:Increase:10"));

        var lastLineButtons = SwitchKeyboardExtensions.BuildLastLine(
            "AS:SwKbdSt",
            commandArguments.StickerPackId.ToString(),
            pageTo,
            countOnPage,
            stickers.Count);
        
        var keyboard = new InlineKeyboardDto(buttons, lastLineButtons);

        return new AddStickerSwitchKeyboardStickersResult(
            commandArguments.ChatId, 
            keyboard,
            commandArguments.BotMessageId);
    }
}