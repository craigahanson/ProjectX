using System.Text.Json.Serialization;

namespace ProjectX.Authentication.Tests
{
    public class TokenError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}