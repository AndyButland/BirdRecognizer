namespace BirdRecognizer.Train.Models
{
    using System.Collections.Generic;

    public class ImagesViewModel
    {
        public IList<Image> Tags { get; set; } = new List<Image>();

        public string Message { get; set; }

        public class Image
        {
            public string Url { get; set; }
        }
    }
}