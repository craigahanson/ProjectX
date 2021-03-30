using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace ProjectX.Authentication.Tests
{
    [TestFixture]
    public class ClientTests
    {
        private const string TokenEndpoint = "https://server/connect/token";
        
        private readonly HttpClient client;
        
        public ClientTests()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);

            client = server.CreateClient();
        }
        
        #region Token
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenGrantTypeIsMissing()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("unsupported_grant_type"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenGrantTypeIsNotValid()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "implicit"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("unsupported_grant_type"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenClientDoesNotExist()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "INVALIDCLIENT"),
                new KeyValuePair<string, string>("client_secret", "secret")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_client"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenClientSecretDoesNotMatch()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "NOTSECRET")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_client"));
        }
        
        [Test]
        public async Task Token_ReturnsOK_WhenClientExistsAndUsesCorrectSecret()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);
            
            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tokenResult = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenResult>();
            Assert.That(tokenResult, Is.Not.Null);
            Assert.That(tokenResult.AccessToken, Is.Not.Null.Or.Empty);
            Assert.That(tokenResult.TokenType, Is.EqualTo("Bearer"));
            Assert.That(tokenResult.ExpiresIn, Is.EqualTo(3600));
            Assert.That(tokenResult.Scope, Is.EqualTo("projectx.rest"));
        }
        
        #endregion
    }

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
    }

    public class TokenError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public static class StringExtensions
    {
        public static T FromJsonAsync<T>(this string json) where T : class
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}