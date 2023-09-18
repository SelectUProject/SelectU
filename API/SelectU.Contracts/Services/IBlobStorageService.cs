using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, Stream content);
        Task<Stream> DownloadFileAsync(string containerName, string blobName);
        Task<bool> DeleteFileAsync(string containerName, string blobName);
        Task<bool> UpdateFileAsync(string containerName, string blobName, Stream content);
    }
}
