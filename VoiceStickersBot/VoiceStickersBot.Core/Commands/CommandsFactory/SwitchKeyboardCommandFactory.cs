﻿using VoiceStickersBot.Core.Commands.SwitchKeyboard;

namespace VoiceStickersBot.Core.Commands.CommandsFactory;

public class SwitchKeyboardCommandFactory : CommandFactoryBase
{
    public override IReadOnlyList<string> CommandPrefixes { get; } = new[] {"pageleft", "pageright"};

    public override ICommand CreateCommand(RequestContext requestContext)
    {
        var pageFrom = int.Parse(requestContext.CommandText.Split(':')[1]);
        //В команду лучше добавить список всех единиц (паков или стикеров),
        //чтото обобщенное чтобы можно было использовать один метод для паков и стикеров
        return new SwitchKeyboardCommand(requestContext, pageFrom, requestContext.CommandText, 10);
    }
}