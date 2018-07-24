namespace BirdRecognizer.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using BirdRecognizer.Common.Services;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;

    public static class ClassifyImage
    {
        [FunctionName("ClassifyImage")]
        public static async Task Run([BlobTrigger("birds/{name}", Connection = "AzureWebJobsStorage")]Stream blob, string name, TraceWriter log)
        {
            var storageService = InstantiateStorageService();
            if (await AlreadyProcessed(storageService, name))
            {
                return;
            }

            var response = await GetPredictionResponse(blob);
            await ApplyPredictionToBlob(storageService, name, response);
        }

        private static AzureBlobStorageService InstantiateStorageService()
        {
            var storageService = new AzureBlobStorageService(
                new AzureBlobStorageServiceOptions
                    {
                        ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                        ContainerName = Environment.GetEnvironmentVariable("ContainerName")
                    });
            return storageService;
        }

        private static async Task<bool> AlreadyProcessed(IStorageService storageService, string name)
        {
            var metadata = await storageService.GetMetadataFromFileAsync(name);
            return metadata[Common.Constants.ImageMetadataKeys.ClassificationStatus] != Common.ImageClassificationStatus.Pending.ToString();
        }

        private static async Task<PredictionResponse> GetPredictionResponse(Stream blob)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", Environment.GetEnvironmentVariable("PredictionKey"));

            var url = Environment.GetEnvironmentVariable("PredictionUrl");

            var byteData = GetImageAsByteArray(blob);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PredictionResponse>(responseString);
            }
        }

        private static byte[] GetImageAsByteArray(Stream blob)
        {
            var binaryReader = new BinaryReader(blob);
            return binaryReader.ReadBytes((int)blob.Length);
        }

        private static async Task ApplyPredictionToBlob(IStorageService storageService, string name, PredictionResponse response)
        {
            var metadata = CreateMetadata(response);
            await WriteMetadataToFile(storageService, name, metadata);
        }

        private static IDictionary<string, string> CreateMetadata(PredictionResponse response)
        {
            return new Dictionary<string, string>
                {
                    {
                        Common.Constants.ImageMetadataKeys.ClassificationStatus,
                        Common.ImageClassificationStatus.Completed.ToString()
                    },
                    {
                        Common.Constants.ImageMetadataKeys.PredictionDetail,
                        GetMetaDataFromPrediction(response)
                    }
                };
        }

        private static string GetMetaDataFromPrediction(PredictionResponse response)
        {
            var predictionDetail = response.Predictions
                .Select(x => new { tag = x.TagName, probability = Math.Round(x.Probability, 4) })
                .ToArray();
            return JsonConvert.SerializeObject(predictionDetail);
        }

        private static async Task WriteMetadataToFile(IStorageService storageService, string name, IDictionary<string, string> metadata)
        {
            await storageService.SetMetadataOnFileAsync(name, metadata);
        }
    }
}
