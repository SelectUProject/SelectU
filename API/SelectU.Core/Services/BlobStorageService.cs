using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            BlobServiceClient blobServiceClient = new BlobServiceClient(_config.ConnectionString);
        }

        public async Task<string> UploadFileAsync(string containerName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string blobName = Guid.NewGuid().ToString(); // Generate a new GUID-based blob name
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true);
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

        public async Task<bool> UpdateFileAsync(string containerName, string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, true);
            return true;
        }
    }
}
