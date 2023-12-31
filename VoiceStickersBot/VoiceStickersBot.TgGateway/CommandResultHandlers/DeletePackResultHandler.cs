﻿using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceStickersBot.Core.CommandArguments;
using VoiceStickersBot.Core.CommandResults;
using VoiceStickersBot.Core.CommandResults.DeletePackResults;
using VoiceStickersBot.Infra.ObjectStorage;

namespace VoiceStickersBot.TgGateway.CommandResultHandlers;

public class DeletePackResultHandler : ICommandResultHandler
{
    public CommandType CommandType => CommandType.DeletePack;

    private readonly ObjectStorageClient objectStorage = new();
    
    private readonly Dictionary<Type, Func<ITelegramBotClient, Dictionary<long, UserInfo>, ICommandResult, Task>> handlers;

    public DeletePackResultHandler()
    {
        handlers = new Dictionary<Type, Func<ITelegramBotClient, Dictionary<long, UserInfo>, ICommandResult, Task>>()
        {
            {
                typeof(DeletePackSwitchKeyboardPacksResult),
                async (bot, infos, res) => await Handle(bot, infos, (DeletePackSwitchKeyboardPacksResult)res)
            },
            {
                typeof(DeletePackSwitchKeyboardStickersResult),
                async (bot, infos, res) => await Handle(bot, infos, (DeletePackSwitchKeyboardStickersResult)res)
            },
            {
                typeof(DeletePackSendStickerResult),
                async (bot, infos, res) => await Handle(bot, infos, (DeletePackSendStickerResult)res)
            },
            {
                typeof(DeletePackDeletePackResult),
                async (bot, infos, res) => await Handle(bot, infos, (DeletePackDeletePackResult)res)
            },
            {
                typeof(DeletePackConfirmResult),
                async (bot, infos, res) => await Handle(bot, infos, (DeletePackConfirmResult)res)
            }
        };
    }

    public Task HandleResult(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        ICommandResult result)
    {
        return handlers[result.GetType()](bot, userInfos, result);
    }
    
    private async Task Handle(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        DeletePackDeletePackResult result)
    {
        userInfos[result.ChatId] = new UserInfo(UserState.NoWait);

        await bot.SendTextMessageAsync(
            result.ChatId,
            "Стикерпак успешно удалён",
            replyMarkup: DefaultKeyboard.CommandsKeyboard);
    }
    
    private async Task Handle(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        DeletePackSwitchKeyboardPacksResult result)
    {
        userInfos[result.ChatId] = new UserInfo(UserState.NoWait);

        var markup = SwitchKeyboardResultExtensions.GetMarkupFromDto(result.KeyboardDto);

        var message = "Выберите набор, который хотите удалить:";
        var botMessageId = result.BotMessageId;
        await BotSendExtensions.SendOrEdit(bot, botMessageId, message, markup, result.ChatId);
        
        /*if (result.BotMessageId is null)
        {
            await bot.SendTextMessageAsync(
                result.ChatId,
                "Выберите набор, который хотите удалить:",
                replyMarkup: markup);
        }
        else
        {
            await bot.EditMessageReplyMarkupAsync(
                inlineMessageId: result.BotMessageId,
                replyMarkup: markup);
        }*/
    }

    private async Task Handle(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        DeletePackSwitchKeyboardStickersResult result)
    {
        userInfos[result.ChatId] = new UserInfo(
            UserState.WaitStickerChoice,
            stickerPackId: result.StickerPackId.ToString());
        
        var markup = SwitchKeyboardResultExtensions.GetMarkupFromDto(result.KeyboardDto);

        var message = "Вот все стикеры из выбранного набора:";
        var botMessageId = result.BotMessageId;
        await BotSendExtensions.SendOrEdit(bot, botMessageId, message, markup, result.ChatId);
        
        /*if (result.BotMessageId is null)
        {
            var msg = await bot.SendTextMessageAsync(
                result.ChatId,
                "Вот все стикеры из выбранного набора:",
                replyMarkup: markup);
        }
        else
        {
            await bot.EditMessageReplyMarkupAsync(
                inlineMessageId: result.BotMessageId,
                replyMarkup: markup);
        }*/
    }

    private async Task Handle(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        DeletePackSendStickerResult result)
    {
        userInfos[result.ChatId] = new UserInfo(
            UserState.WaitStickerChoice,
            stickerPackId: result.StickerPackId.ToString());
        
        var memoryStream = await objectStorage.GetObjectFromStorage(ObjectLocation.Parse(result.Sticker.Location));
        var voiceFile = InputFile.FromStream(memoryStream);
        await bot.SendVoiceAsync(
            result.ChatId,
            voiceFile);
    }
    
    private async Task Handle(
        ITelegramBotClient bot,
        Dictionary<long, UserInfo> userInfos,
        DeletePackConfirmResult result)
    {
        userInfos[result.ChatId] = new UserInfo(UserState.NoWait);

        var markup = SwitchKeyboardResultExtensions.GetMarkupFromDto(result.KeyboardDto);
        
        var message = "Вы точно хотите удалить этот набор?";
        var botMessageId = result.BotMessageId;
        await BotSendExtensions.SendOrEdit(bot, botMessageId, message, markup, result.ChatId);
        
        /*await bot.SendTextMessageAsync(
            result.ChatId,
            "Вы точно хотите удалить этот набор?",
            replyMarkup: markup);*/
    }
}