namespace BirdRecognizer.Predict.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class UploadViewModel
    {
        [Required(ErrorMessage = "Please select a file to upload.")]
        public IFormFile File { get; set; }
    }
}
