// <copyright file="INetworkStorageService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace GameOn.External.NetworkStorage.Interfaces
{
    /// <summary>
    /// INetworkStorageService interface.
    /// </summary>
    public interface INetworkStorageService
    {
        Task<Stream?> GetFile(string bucketName, string fileName);

        Task UploadFile(string bucketName, string filePath, IFormFile file);

        String GetContentType(string fileName);
    }
}
