using System.Text;
using System.Text.Json;

namespace Multipay.Receivable.Microservice.Api.Domain.SeedWork
{
    public static class Base64Converter
    {
        public static string ToBase64String(this string input)
        {
            try
            {
                byte[] inputAsByteArray = Encoding.ASCII.GetBytes(input);
                return Convert.ToBase64String(inputAsByteArray);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception object : {JsonSerializer.Serialize(e)}. An error occurred while converting to Base64: {e.Message}");
                return string.Empty;
            }
        }

        public static string ConvertToBase64Url(byte[] input)
        {
            try
            {
                string base64 = Convert.ToBase64String(input);

                return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception object : {JsonSerializer.Serialize(e)}. An error occurred while converting to Base64: {e.Message}");
                return string.Empty;
            }
        }

        public static byte[] ConvertFromBase64Url(string base64Url)
        {
            try
            {
                if (string.IsNullOrEmpty(base64Url))
                    throw new ArgumentException("Input Base64Url byte array cannot be null or empty.");


                string base64String = base64Url.Replace("-", "+").Replace("_", "/");

                int padding = 4 - (base64String.Length % 4);
                if (padding < 4)
                {
                    base64String = base64String.PadRight(base64String.Length + padding, '=');
                }

                byte[] decodedBytes = Convert.FromBase64String(base64String);
                return decodedBytes;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[Error] Base64 decoding failed: {e}");
                return [];
            }
        }
    }
}
