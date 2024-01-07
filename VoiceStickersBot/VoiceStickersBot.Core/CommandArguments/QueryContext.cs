﻿namespace VoiceStickersBot.Core.CommandArguments;

public class QueryContext
{
    public string CommandType { get; }

    public string CommandStep { get; }

    public List<string> CommandArguments { get; }

    public long ChatId { get; }
    public int? BotMessageId { get; }

    public QueryContext(
        string commandType,
        string commandStep,
        List<string> commandArguments,
        long chatId,
        int? botMessageId = null)
    {
        CommandType = commandType;
        CommandStep = commandStep;
        CommandArguments = commandArguments;

        BotMessageId = botMessageId;
        ChatId = chatId;
    }
}