﻿namespace VoiceStickersBot.Infra.ObjectStorage;

public interface IObjectStorageClient
{
    public Task<MemoryStream> GetObjectFromStorage(ObjectLocation location);

    public Task<ObjectLocation> PutObjectInStorage(string path, Guid objectId, string contentType, byte[] objBytes);
}