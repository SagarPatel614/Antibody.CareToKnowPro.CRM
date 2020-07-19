using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Antibody.CareToKnowPro.CRM.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        CloudBlobClient _blobClient;
        public AzureStorageService(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobClient.DefaultRequestOptions = new BlobRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(3), 4)
            };
        }

        private readonly SharedAccessBlobPolicy _sharedAccessBlobPolicyNoExpiry = new SharedAccessBlobPolicy
        {
            SharedAccessExpiryTime = new DateTime(2100, 1, 1).ToUniversalTime()
        };

        public async Task<bool> DeleteBlobAsync(string blobUrl)
        {
            var cloudBlockBlob = await _blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));
            return await cloudBlockBlob.DeleteIfExistsAsync();
        }

        public async Task<string> GetBlobContentTypeAsync(string blobUrl)
        {
            var cloudBlockBlob = await _blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));
            if (await cloudBlockBlob.ExistsAsync())
            {
                return cloudBlockBlob.Properties.ContentType;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<CloudBlockBlob> GetBlobReference(string blobUrl)
        {
            var cloudBlockBlob = await _blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));
            if (await cloudBlockBlob.ExistsAsync())
            {
                return cloudBlockBlob as CloudBlockBlob;
            }

            else
            {
                return null;
            }
        }

        public async Task<Stream> GetBlobStreamAsync(string blobUrl)
        {
            var cloudBlockBlob = await _blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));

            if (await cloudBlockBlob.ExistsAsync())
            {
                MemoryStream stream = new MemoryStream();
                await cloudBlockBlob.DownloadToStreamAsync(stream);
                return stream;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> UploadToBlobAsync(IFormFile file, string containerName, string directoryName, string blobName, bool isImage)
        {
            const int maxSizeInBytes = 15 * 1024 * 1024; // 15 KB * 1024 = 15 * 1024 * 1024 = 15 MB

            using (var stream = file.OpenReadStream())
            {
                stream.Seek(0, 0);

                var cloudBlockBlob = await GetBlobReference(containerName, directoryName, blobName);

                // Azure Storage SDK does not treat a zero-byte source as an error when running against the real storage service
                // This behaviour differs when running against the local emulator (treated as a failure)
                if (cloudBlockBlob.StreamWriteSizeInBytes == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(stream), $"Data from Stream is empty.");
                }

                // Fail early on max size exceeded, when possible
                if (cloudBlockBlob.StreamWriteSizeInBytes > maxSizeInBytes)
                {
                    throw new ArgumentOutOfRangeException(nameof(stream), $"Data cannot exceed {maxSizeInBytes} bytes.");
                }

                if (!string.IsNullOrEmpty(file.ContentType))
                {
                    cloudBlockBlob.Properties.ContentType = file.ContentType;
                }

                if (isImage)
                {
                    using (Image img = Image.FromStream(stream))
                    {

                        using (Bitmap b = new Bitmap(img, ResizeKeepAspect(img)))
                        {
                            var size = ResizeKeepAspect(img);
                            using (MemoryStream ms2 = new MemoryStream())
                            {
                                b.Save(ms2, img.RawFormat);
                                ms2.Seek(0, 0);
                                await cloudBlockBlob.UploadFromStreamAsync(ms2, ms2.Length);
                            }
                        }
                    }
                }
                else
                {
                    await cloudBlockBlob.UploadFromStreamAsync(stream, stream.Length);
                }

                return cloudBlockBlob.StorageUri.PrimaryUri.ToString();

            }
        }

        private Size ResizeKeepAspect(Image image, int maxWidth = 2000, int maxHeight = 2000, bool enlarge = false)
        {
            maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, image.Width);
            maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, image.Height);

            decimal rnd = Math.Min(maxWidth / (decimal)image.Width, maxHeight / (decimal)image.Height);
            return new Size((int)Math.Round(image.Width * rnd), (int)Math.Round(image.Height * rnd));
        }

        public async Task<long> GetBlobSizeAsync(string containerName, string directoryName, string blobName)
        {
            var blob = await GetBlobReference(containerName, directoryName, blobName);

            await blob.FetchAttributesAsync();

            return blob.Properties.Length;
        }

        public async Task<string> GetBlobContentTypeAsync(string containerName, string directoryName, string blobName)
        {
            var blob = await GetBlobReference(containerName, directoryName, blobName);

            await blob.FetchAttributesAsync();

            return blob.Properties.ContentType;
        }

        private async Task<CloudBlockBlob> GetBlobReference(string containerName, string directoryName, string blobName)
        {
            var cloudBlobContainer = _blobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            var cloudDirectory = cloudBlobContainer.GetDirectoryReference(directoryName);
            return cloudDirectory.GetBlockBlobReference(blobName);
            
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string directoryName, string blobName)
        {
            var blob = await GetBlobReference(containerName, directoryName, blobName);

            return await blob.OpenReadAsync();
        }

        public async Task<Uri> GetUrlForCreateAsync(string containerName, string directoryName, string blobName)
        {
            const string storedPolicyName = "Create";

            // NOTE: Returning an awaited Task to resolve compiler warning CS1998 about this async method lacking 'await' operators, since
            //       the 'IStorageService' interface declares this method as async but no async calls are actually needed for this implementation
            return await Task.Run(() => GetUrl(containerName, directoryName, blobName, storedPolicyName));
        }

        public async Task<Uri> GetUrlForReadAsync(string containerName, string directoryName, string blobName)
        {
            const string storedPolicyName = "Read";

            // NOTE: Returning an awaited Task to resolve compiler warning CS1998 about this async method lacking 'await' operators, since
            //       the 'IStorageService' interface declares this method as async but no async calls are actually needed for this implementation
            return await Task.Run(() => GetUrl(containerName, directoryName, blobName, storedPolicyName));
        }

        private async Task<Uri> GetUrl(string containerName, string directoryName, string blobName, string storedPolicyName)
        {
            var blob = await GetBlobReference(containerName, directoryName, blobName);

            var sas = blob.GetSharedAccessSignature(_sharedAccessBlobPolicyNoExpiry, storedPolicyName);

            return new Uri($"{blob.Uri.AbsoluteUri}{sas}");
        }
    }
}
