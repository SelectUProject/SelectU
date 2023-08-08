using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Services
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(string blobName, Stream content);
        Task<Stream> DownloadFileAsync(string blobName);
        Task<bool> DeleteFileAsync(string blobName);
        Task<bool> UpdateFileAsync(string blobName, Stream content);
    }
}
