namespace BirdRecognizer.Common.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStorageService
    {
        Task UploadFileAsync(string fileName, byte[] bytes, string contentType);

        Task<IDictionary<string, string>> GetMetadataFromFileAsync(string fileName);

        Task SetMetadataOnFileAsync(string fileName, IDictionary<string, string> metadata);
    }
}
