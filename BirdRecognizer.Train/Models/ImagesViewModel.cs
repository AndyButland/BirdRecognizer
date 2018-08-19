namespace BirdRecognizer.Train.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;

    public class ImagesViewModel
    {
        [DisplayName("Tag: ")]
        public Guid? SelectedTag { get; set; }
        
        public SelectList Tags { get; set; }

        public IList<Image> Images { get; set; } = new List<Image>();

        public string Message { get; set; }

        public class Image
        {
            public string Url { get; set; }
        }
    }
}