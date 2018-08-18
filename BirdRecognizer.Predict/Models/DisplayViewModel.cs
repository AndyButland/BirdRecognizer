namespace BirdRecognizer.Predict.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class DisplayViewModel
    {
        public string ImageUrl { get; set; }

        public string ClassificationStatus { get; set; }

        public IList<PredictionDetail> Predictions { get; set; } = new List<PredictionDetail>();

        public string PredictionSummary
        {
            get
            {
                if (!Predictions.Any())
                {
                    return string.Empty;
                }

                var topPrediction = Predictions
                    .OrderByDescending(x => x.Probability)
                    .First();

                if (topPrediction.Probability > 0.95)
                {
                    return $"I'm pretty sure that's a {topPrediction.Tag}";
                }

                if (topPrediction.Probability > 0.85)
                {
                    return $"Not sure, but I think it might be a {topPrediction.Tag}";
                }

                return "Sorry, I can't make that one out.";
            }
        }

        public class PredictionDetail
        {
            public string Tag { get; set; }

            public double Probability { get; set; }

            public string ProbabilityForDisplay => $"{Probability * 100}%";
        }
    }
}
