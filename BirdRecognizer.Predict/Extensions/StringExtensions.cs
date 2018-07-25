namespace BirdRecognizer.Predict.Extensions
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
