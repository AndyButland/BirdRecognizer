namespace BirdRecognizer.Train.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using BirdRecognizer.Train.Models;
    using Microsoft.Cognitive.CustomVision.Training;
    using Microsoft.Cognitive.CustomVision.Training.Models;

    public class ImagesController : BaseController
    {
        public async Task<ActionResult> Index(Guid? selectedTag)
        {
            var vm = new ImagesViewModel
                {
                    Message = TempData["Message"]?.ToString()
                };

            var api = CreateTrainingApi();
            vm.Tags = await GetTagSelectList(api, selectedTag);

            if (!selectedTag.HasValue)
            {
                return View(vm);
            }

            var images = await api.GetTaggedImagesAsync(ProjectId, 
                tagIds: new List<string> { selectedTag.Value.ToString() });
            foreach (var image in images)
            {
                vm.Images.Add(new ImagesViewModel.Image
                    {
                        Url = image.ThumbnailUri
                    });
            }

            return View(vm);
        }

        private async Task<SelectList> GetTagSelectList(TrainingApi api, Guid? selectedTag = null)
        {
            var tagList = await api.GetTagsAsync(ProjectId);
            return new SelectList(tagList.Tags, "Id", "Name", selectedTag);
        }

        public async Task<ActionResult> Create()
        {
            var vm = new CreateImageViewModel();

            var api = CreateTrainingApi();
            vm.Tags = await GetTagSelectList(api);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateImageViewModel vm)
        {
            var api = CreateTrainingApi();

            if (vm.ImageFiles == null || !vm.ImageFiles.Any())
            {
                ModelState.AddModelError("ImageFiles", "Please select one or more image files.");
            }

            if (ModelState.IsValid)
            {
                var selectedTag = await api.GetTagAsync(ProjectId, vm.TagId);
           
                var imageFiles = vm.ImageFiles
                    ?.Select(x => new ImageFileCreateEntry(x.FileName, GetPostedFileBytes(x)))
                    .ToList();
                await api.CreateImagesFromFilesAsync(ProjectId, 
                    new ImageFileCreateBatch(imageFiles, new List<Guid> { selectedTag.Id }));

                TempData["Message"] = $"Image added with tag {selectedTag.Name}";
                return RedirectToAction("Index", new { selectedTag = selectedTag.Id });
            }

            vm.Tags = await GetTagSelectList(api);
            return View(vm);
        }

        private static byte[] GetPostedFileBytes(HttpPostedFileBase postedFile)
        {
            using (var binaryReader = new BinaryReader(postedFile.InputStream))
            {
                return binaryReader.ReadBytes(postedFile.ContentLength);
            }
        }
    }
}
