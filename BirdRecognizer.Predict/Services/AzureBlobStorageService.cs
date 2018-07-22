namespace BirdRecognizer.Predict.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureBlobStorageService : IStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlobStorageService(IOptions<AzureBlobStorageServiceOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
            _containerName = options.Value.ContainerName;
        }

        public async Task UploadFile(string fileName, byte[] bytes, string contentType)
        {
            var blobClient = GetClient();
            var container = await GetOrCreateContainer(blobClient);
            await CreateAndUploadBlob(fileName, bytes, contentType, container);
        }

        public async Task<IDictionary<string, string>> GetMetadataFromFile(string fileName)
        {
            var blobClient = GetClient();
            var container = await GetOrCreateContainer(blobClient);
            if (!await container.ExistsAsync())
            {
                return null;
            }

            var blob = container.GetBlockBlobReference(fileName);
            if (!await blob.ExistsAsync())
            {
                return null;
            }

            await blob.FetchAttributesAsync();
            return blob.Metadata;
        }

        private CloudBlobClient GetClient()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            return storageAccount.CreateCloudBlobClient();
        }

        private async Task<CloudBlobContainer> GetOrCreateContainer(CloudBlobClient blobClient)
        {
            var container = blobClient.GetContainerReference(_containerName);
            await container.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob };
            await container.SetPermissionsAsync(permissions);
            return container;
        }

        private static async Task CreateAndUploadBlob(string fileName, byte[] bytes, string contentType, CloudBlobContainer container)
        {
            var blob = container.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = contentType;

            blob.Metadata.Add(Constants.ImageMetadataKeys.ClassificationStatus, ImageClassificationStatus.Pending.ToString());

            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }
    }
}
