namespace BirdRecognizer.Common.Services
{
    public class AzureBlobStorageServiceOptions
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
