namespace BirdRecognizer.Train.Models
{
    using System;
    using System.Collections.Generic;

    public class TagsViewModel
    {
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        public string Message { get; set; }

        public class Tag
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}