// <copyright file="INetworkStorageService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.NetworkStorage.Interfaces
{
    /// <summary>
    /// INetworkStorageService interface.
    /// </summary>
    public interface INetworkStorageService
    {
        Task<Stream?> GetFile(string bucketName, string fileName);

        String GetContentType(string fileName);
    }
}
