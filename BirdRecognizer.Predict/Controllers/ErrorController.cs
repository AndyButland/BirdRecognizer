namespace BirdRecognizer.Predict.Controllers
{
    using System.Diagnostics;
    using BirdRecognizer.Predict.Models;
    using Microsoft.AspNetCore.Mvc;
    
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
