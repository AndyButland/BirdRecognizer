namespace BirdRecognizer.Predict.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BirdRecognizer.Common;
    using BirdRecognizer.Common.Services;
    using BirdRecognizer.Predict.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class DisplayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;

        public DisplayController(IConfiguration configuration, IStorageService storageService)
        {
            _configuration = configuration;
            _storageService = storageService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var model = new DisplayViewModel
                {
                    ImageUrl = ConstructImageUrl(id),                
                };

            var imageMetadata = await _storageService.GetMetadataFromFile(id + ".jpg");
            if (imageMetadata != null)
            {
                model.ClassificationStatus = imageMetadata[Constants.ImageMetadataKeys.ClassificationStatus];
            }
            
            return View(model);
        }

        private string ConstructImageUrl(Guid id)
        {
            return $"{_configuration["Application:ImageRootUrl"]}/{id}.jpg";
        }
    }
}
