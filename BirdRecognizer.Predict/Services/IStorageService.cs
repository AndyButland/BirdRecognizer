namespace BirdRecognizer.Predict.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStorageService
    {
        Task UploadFile(string fileName, byte[] bytes, string contentType);

        Task<IDictionary<string, string>> GetMetadataFromFile(string fileName);
    }
}
