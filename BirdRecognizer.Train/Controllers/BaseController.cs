namespace BirdRecognizer.Train.Controllers
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;

    using Microsoft.Cognitive.CustomVision.Training;

    public abstract class BaseController : Controller
    {
        private readonly string _apiKey = ConfigurationManager.AppSettings["CustomVisionServiceTrainingApiKey"];
        protected readonly Guid ProjectId = Guid.Parse(ConfigurationManager.AppSettings["CustomVisionServiceProjectId"]);

        protected TrainingApi CreateTrainingApi()
        {
            return new TrainingApi { ApiKey = _apiKey };
        }
    }
}
