// <copyright file="MinIOService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.NetworkStorage.Implementations
{
    using GameOn.External.NetworkStorage.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Minio;
    using Minio.DataModel.Args;

    /// <summary>
    /// MinIOService class.
    /// </summary>
    public class MinIOService : INetworkStorageService
    {
        private readonly IMinioClient minioClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinIOService" /> class.
        /// </summary>
        /// <param name="minioClient">Minio client interface, injected.</param>
        public MinIOService(IMinioClient minioClient)
        {
            this.minioClient = minioClient;
        }

        /// <inheritdoc />
        public async Task<Stream?> GetFile(string bucketName, string fileName)
        {
            // Creating bucket if it doesn't exist
            var bucketExists = await this.minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));

            if (!bucketExists)
            {
                await this.minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            // Checking if file exists
            try
            {
                var fileExists =
                    await this.minioClient.StatObjectAsync(new StatObjectArgs().WithBucket(bucketName)
                        .WithObject(fileName));

                if (fileExists is null)
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

            var fileStream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
            await this.minioClient.GetObjectAsync(
                new GetObjectArgs().WithBucket(bucketName).WithObject(fileName).WithCallbackStream(stream =>
                {
                    stream.CopyTo(fileStream);
                    fileStream.Position = 0;
                }));

            return fileStream;
        }

        /// <inheritdoc />
        public async Task UploadFile(string bucketName, string filePath, IFormFile file)
        {
            // Creating bucket if it doesn't exist
            var bucketExists = await this.minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));

            if (!bucketExists)
            {
                await this.minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            try
            {
                // Upload file to bucket
                await this.minioClient.PutObjectAsync(
                    new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(filePath)
                        .WithContentType(file.ContentType)
                        .WithStreamData(file.OpenReadStream())
                        .WithObjectSize(file.Length));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
