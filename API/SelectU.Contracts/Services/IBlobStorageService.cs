using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadPhotoAsync(string containerName, IFormFile content)
        Task<string> UploadFileAsync(string containerName, IFormFile content);
        Task<Stream> DownloadFileAsync(string containerName, string blobName);
        Task<bool> DeleteFileAsync(string containerName, string blobName);
        Task<bool> UpdateFileAsync(string containerName, string blobName, IFormFile content);
    }
}
