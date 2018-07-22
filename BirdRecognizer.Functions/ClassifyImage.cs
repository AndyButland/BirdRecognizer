namespace BirdRecognizer.Functions
{
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Newtonsoft.Json;

    public static class ClassifyImage
    {
        [FunctionName("ClassifyImage")]
        public static async Task Run([BlobTrigger("birds/{name}", Connection = "AzureWebJobsStorage")]Stream blob, string name, TraceWriter log)
        {
            var response = await GetPredictionResponse(blob);
        }

        private static async Task<PredictionResponse> GetPredictionResponse(Stream blob)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", "13hc77781f7e4b19b5fcdd72a8df7156");

            var url = "http://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/prediction/d16e136c-5b0b-4b84-9341-6a3fff8fa7fe/image?iterationId=f4e573f6-9843-46db-8018-b01d034fd0f2";

            HttpResponseMessage response;

            var byteData = GetImageAsByteArray(blob);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PredictionResponse>(responseString);
            }
        }

        private static byte[] GetImageAsByteArray(Stream blob)
        {
            var binaryReader = new BinaryReader(blob);
            return binaryReader.ReadBytes((int)blob.Length);
        }
    }
}
