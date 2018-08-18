namespace BirdRecognizer.Functions.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using BirdRecognizer.Common.Windows;
    using BirdRecognizer.Common.Windows.Services;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Cognitive.CustomVision.Prediction;
    using Microsoft.Cognitive.CustomVision.Prediction.Models;
    using Newtonsoft.Json;

    public static class ClassifyImage
    {
        [FunctionName("ClassifyImage")]
        public static async Task Run([BlobTrigger("birds/{name}", 
                                     Connection = "AzureWebJobsStorage")]Stream blob, 
                                     string name)
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
                new AzureBlobStorageServiceArgs
                    {
                        ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                        ContainerName = Environment.GetEnvironmentVariable("ContainerName")
                    });
            return storageService;
        }

        private static async Task<bool> AlreadyProcessed(IStorageService storageService, string name)
        {
            var metadata = await storageService.GetMetadataFromFileAsync(name);
            return metadata[Constants.ImageMetadataKeys.ClassificationStatus] 
                != ImageClassificationStatus.Pending.ToString();
        }

        private static async Task<ImagePredictionResultModel> GetPredictionResponse(Stream blob)
        {
            var endpoint = new PredictionEndpoint
                {
                    ApiKey = Environment.GetEnvironmentVariable("PredictionKey")
                };

            var projectId = Guid.Parse(Environment.GetEnvironmentVariable("ProjectId"));
            return await endpoint.PredictImageAsync(projectId, blob);
        }
        
        private static async Task ApplyPredictionToBlob(IStorageService storageService, 
                                                        string name,
                                                        ImagePredictionResultModel response)
        {
            var metadata = CreateMetadata(response);
            await WriteMetadataToFile(storageService, name, metadata);
        }

        private static IDictionary<string, string> CreateMetadata(ImagePredictionResultModel response)
        {
            return new Dictionary<string, string>
                {
                    {
                        Constants.ImageMetadataKeys.ClassificationStatus,
                        ImageClassificationStatus.Completed.ToString()
                    },
                    {
                        Constants.ImageMetadataKeys.PredictionDetail,
                        GetMetaDataFromPrediction(response)
                    }
                };
        }

        private static string GetMetaDataFromPrediction(ImagePredictionResultModel response)
        {
            var predictionDetail = response.Predictions
                .Select(x => new { tag = x.Tag, probability = Math.Round(x.Probability, 4) })
                .ToArray();
            return JsonConvert.SerializeObject(predictionDetail);
        }

        private static async Task WriteMetadataToFile(IStorageService storageService, string name, IDictionary<string, string> metadata)
        {
            await storageService.SetMetadataOnFileAsync(name, metadata);
        }
    }
}
