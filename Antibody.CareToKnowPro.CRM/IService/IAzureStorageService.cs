using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Antibody.CareToKnowPro.CRM.IService
{
    public interface IAzureStorageService
    {
        Task<string> UploadToBlobAsync(IFormFile stream, string containerName, string directoryName, string blobName, bool isImage = false);
        Task<string> GetBlobContentTypeAsync(string blobUrl);
        Task<Stream> GetBlobStreamAsync(string blobUrl);
        Task<CloudBlockBlob> GetBlobReference(string blobUrl);
        Task<bool> DeleteBlobAsync(string blobUrl);
    }
}
