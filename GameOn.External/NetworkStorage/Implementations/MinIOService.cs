// <copyright file="MinIOService.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

using GameOn.External.NetworkStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;

namespace GameOn.External.NetworkStorage.Implementations
{
    /// <summary>
    /// MinIOService class.
    /// </summary>
    public class MinIOService : INetworkStorageService
    {
        private IMinioClient minioClient;

        public MinIOService(IMinioClient minioClient)
        {
            this.minioClient = minioClient;
        }

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
                throw ex;
            }
        }

        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {
                case ".txt":
                    return "text/plain";
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/x-rar-compressed";
                case ".mp3":
                    return "audio/mpeg";
                case ".mp4":
                    return "video/mp4";
                case ".webp":
                    return "image/webp";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
