using System.Security.Principal;
using System.Text.Json.Serialization;

namespace ProjectX.Authentication.Tests
{
    public class UserInfoResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
    }
}