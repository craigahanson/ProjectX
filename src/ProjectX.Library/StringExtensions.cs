using System.Text.Json;

namespace ProjectX.Library
{
    public static class StringExtensions
    {
        public static T FromJsonAsync<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}