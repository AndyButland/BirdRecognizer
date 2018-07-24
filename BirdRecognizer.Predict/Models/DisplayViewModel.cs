namespace BirdRecognizer.Predict.Models
{
    using System.Collections.Generic;

    public class DisplayViewModel
    {
        public string ImageUrl { get; set; }

        public string ClassificationStatus { get; set; }

        public IList<PredictionDetail> Predictions { get; set; }

        public class PredictionDetail
        {
            public string Tag { get; set; }

            public double Probability { get; set; }

            public string ProbabilityForDisplay => $"{Probability * 100}%";
        }
    }
}
