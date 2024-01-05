﻿using VoiceStickersBot.Core.CommandArguments;
using VoiceStickersBot.Core.CommandArguments.DeletePackCommandArguments;
using VoiceStickersBot.Core.CommandResults;
using VoiceStickersBot.Core.Repositories.StickerPacksRepository;
using VoiceStickersBot.Core.Repositories.UsersRepository;

namespace VoiceStickersBot.Core.CommandHandlers.CommandHandlers.DeletePackHandlers;

public class DeletePackDeletePackHandler : ICommandHandler
{
    public CommandType CommandType => CommandType.DeletePack;

    private readonly DeletePackDeletePackArguments commandArguments;
    private readonly IStickerPacksRepository stickerPacksRepository;
    private readonly IUsersRepository usersRepository;

    public DeletePackDeletePackHandler(
        DeletePackDeletePackArguments commandArguments,
        IUsersRepository usersRepository,
        IStickerPacksRepository stickerPacksRepository)
    {
        this.commandArguments = commandArguments;
        this.usersRepository = usersRepository;
        this.stickerPacksRepository = stickerPacksRepository;
    }

    public async Task<ICommandResult> Handle()
    {
        //TODO: в StickerPacksRepository и UsersRepository надо метод DeletePack
        throw new NotImplementedException();
    }
}