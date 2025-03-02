namespace PricePredict.Shared.Constants
{
    public static class ValidationMessages
    {
        public static string NotNullOrEmpty(string fieldName)
        {
            return $"{fieldName} cannot be null or empty.";
        }
    }
}
