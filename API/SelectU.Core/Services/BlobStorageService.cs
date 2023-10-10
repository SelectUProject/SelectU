using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using SelectU.Contracts.Config;
using SelectU.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Core.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureBlobSettingsConfig _config;
        public BlobStorageService(IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig)
        {
            _config = azureBlobSettingsConfig.Value;
            _blobServiceClient = new BlobServiceClient(_config.ConnectionString);
        }
        public async Task<string> UploadPhotoAsync(string containerName, IFormFile content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string blobName = $"{Guid.NewGuid()}/{content.FileName}"; // Generate a new GUID-based blob name
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = content.OpenReadStream())
            {
                var response = await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.AbsoluteUri;
        }
        public async Task<string> UploadFileAsync(string containerName, IFormFile content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string blobName = $"{Guid.NewGuid()}/{content.FileName}"; // Generate a new GUID-based blob name
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = content.OpenReadStream())
            {
               var response = await blobClient.UploadAsync(stream, true);
            }

            var blobUrl = blobClient.Uri.AbsoluteUri;

            return blobName;
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Content;
        }

        public async Task<bool> DeleteFileAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> UpdateFileAsync(string containerName, string blobName, IFormFile content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = content.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            return true;
        }
    }
}
