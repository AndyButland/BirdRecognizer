namespace BirdRecognizer.Predict.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using BirdRecognizer.Common.Services;
    using BirdRecognizer.Predict.Models;
    using Microsoft.AspNetCore.Mvc;

    [AutoValidateAntiforgeryToken]
    public class UploadController : Controller
    {
        private readonly IStorageService _storageService;

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Index(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = Guid.NewGuid();

                await UploadFileToStorage(model, imageId);

                return RedirectToAction("Index", "Display", new { id = imageId });
            }

            return View();
        }

        private async Task UploadFileToStorage(UploadViewModel model, Guid imageId)
        {
            using (var ms = new MemoryStream())
            {
                await model.File.CopyToAsync(ms);
                await _storageService.UploadFileAsync(imageId + ".jpg", ms.ToArray(), "image/jpeg");
            }
        }
    }
}
