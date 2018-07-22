namespace BirdRecognizer.Predict
{
    public enum ImageClassificationStatus
    {
        Pending,
        Completed,
        Failed
    }

    public static class Constants
    {
        public static class ImageMetadataKeys
        {
            public static readonly string ClassificationStatus = "ClassificationStatus";
        }
    }
}
