namespace BirdRecognizer.Predict.Services
{
    using System.Threading.Tasks;

    public interface IStorageService
    {
        Task UploadFile(string fileName, byte[] bytes, string contentType);
    }
}
