using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using SelectU.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Core.Services
{
    public class AzureBlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        public AzureBlobStorageService(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            _containerName = configuration["AzureBlobSettings:ContainerName"] ?? "";
        }

        public async Task UploadFileAsync(string blobName, Stream content)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, true);
        }

        public async Task<Stream> DownloadFileAsync(string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Content;
        }

        public async Task<bool> DeleteFileAsync(string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> UpdateFileAsync(string blobName, Stream content)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, true);
            return true;
        }
    }
}
