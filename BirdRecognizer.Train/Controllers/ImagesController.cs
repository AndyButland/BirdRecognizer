namespace BirdRecognizer.Train.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BirdRecognizer.Train.Models;

    public class ImagesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var vm = new ImagesViewModel
                {
                    Message = TempData["Message"]?.ToString()
                };

            return View(vm);
        }
    }
}
