namespace BirdRecognizer.Train.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class CreateImageViewModel
    {
        [Required(ErrorMessage = "Please select a tag")]
        [DisplayName("Tag")]
        public Guid TagId { get; set; }
        
        public SelectList Tags { get; set; }

        [DisplayName("Image file(s)")]
        public IEnumerable<HttpPostedFileBase> ImageFiles { get; set; }
    }
}