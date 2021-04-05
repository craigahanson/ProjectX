using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectX.Authentication.Tests
{
    public class TokenResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        public IEnumerable<string> Scopes => Scope.Split(" ");
    }
}