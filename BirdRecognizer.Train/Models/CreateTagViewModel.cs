namespace BirdRecognizer.Train.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTagViewModel
    {
        [Required(ErrorMessage = "Please enter the name of the tag")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}