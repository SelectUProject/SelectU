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
        private readonly BlobContainerClient _containerClient;
        public BlobStorageService(IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig)
        {
            AzureBlobSettingsConfig config = azureBlobSettingsConfig.Value;
            BlobServiceClient blobServiceClient = new BlobServiceClient(config.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
        }

        public async Task UploadFileAsync(string blobName, Stream content)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, true);
        }

        public async Task<Stream> DownloadFileAsync(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Content;
        }

        public async Task<bool> DeleteFileAsync(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> UpdateFileAsync(string blobName, Stream content)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(content, true);
            return true;
        }
    }
}
