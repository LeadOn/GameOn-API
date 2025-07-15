// <copyright file="INetworkStorageService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.NetworkStorage.Interfaces
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// INetworkStorageService interface.
    /// </summary>
    public interface INetworkStorageService
    {
        /// <summary>
        /// Get files from S3 bucket.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Stream.</returns>
        Task<Stream?> GetFile(string bucketName, string fileName);

        /// <summary>
        /// Upload file to bucket.
        /// </summary>
        /// <param name="bucketName">Bucket name.</param>
        /// <param name="filePath">Path to file.</param>
        /// <param name="file">File from form.</param>
        /// <returns>Nothing.</returns>
        Task UploadFile(string bucketName, string filePath, IFormFile file);

        /// <summary>
        /// Get content type by file name.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Content type (like image/png).</returns>
        string GetContentType(string fileName);
    }
}
