namespace BirdRecognizer.Train.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BirdRecognizer.Train.Models;
    using Microsoft.Cognitive.CustomVision.Training;

    public class ImagesController : BaseController
    {
        public async Task<ActionResult> Index(Guid? selectedTag)
        {
            var vm = new ImagesViewModel
                {
                    Message = TempData["Message"]?.ToString()
                };

            var api = CreateTrainingApi();
            var tagList = await api.GetTagsAsync(ProjectId);
            vm.Tags = new SelectList(tagList.Tags, "Id", "Name", selectedTag);

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
    }
}
