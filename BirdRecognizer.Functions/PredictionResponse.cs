namespace BirdRecognizer.Functions
{
    using System;
    using System.Collections.Generic;

    /*
        Response as JSON:
        {
            "Id": "string",
            "Project": "string",
            "Iteration": "string",
            "Created": "string",
            "Predictions": [
                {
                    "Probability": 0.0,
                    "TagId": "string",
                    "TagName": "string",
                    "Region": {
                    "Left": 0.0,
                    "Top": 0.0,
                    "Width": 0.0,
                    "Height": 0.0
                    }
                }
            ]
        }
    */
    public class PredictionResponse
    {
        public string Id { get; set; }

        public string Project { get; set; }

        public string Iteration { get; set; }

        public DateTime Created { get; set; }

        public List<Prediction> Predictions { get; set; }

        public class Prediction
        {
            public string TagId { get; set; }

            public string TagName { get; set; }

            public double Probability { get; set; }
        }
    }
}
