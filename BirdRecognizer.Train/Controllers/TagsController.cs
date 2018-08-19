namespace BirdRecognizer.Train.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BirdRecognizer.Train.Models;
    using Microsoft.Cognitive.CustomVision.Training;

    public class TagsController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var vm = new TagsViewModel
                {
                    Message = TempData["Message"]?.ToString()
                };

            var api = CreateTrainingApi();
            var tagList = await api.GetTagsAsync(ProjectId);
            
            foreach (var tag in tagList.Tags.OrderBy(x => x.Name))
            {
                vm.Tags.Add(new TagsViewModel.Tag
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
            }

            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTagViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var api = CreateTrainingApi();
                await api.CreateTagAsync(ProjectId, vm.Name, vm.Description);

                TempData["Message"] = "Tag created";
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var api = CreateTrainingApi();

            var tag = await api.GetTagAsync(ProjectId, id);
            if (tag == null)
            {
                return HttpNotFound();
            }

            var vm = new EditTagViewModel
                {
                    Id = id,
                    Name = tag.Name,
                    Description = tag.Description
                };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditTagViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var api = CreateTrainingApi();

                var tag = await api.GetTagAsync(ProjectId, vm.Id);
                tag.Name = vm.Name;
                tag.Description = vm.Description;

                await api.UpdateTagAsync(ProjectId, vm.Id, tag);

                TempData["Message"] = "Tag updated";
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
