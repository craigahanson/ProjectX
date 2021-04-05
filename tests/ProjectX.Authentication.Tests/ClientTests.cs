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
using ProjectX.Library;

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
        public async Task Token_ReturnsBadRequest_WhenScopeIsInvalid()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "INVALIDSCOPE")
            });
            
            //Act
            var httpResponseMessage = await client.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_scope"));
        }
        
        [Test]
        public async Task Token_ReturnsOK_WhenClientExistsAndUsesCorrectSecret()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "projectx.rest")
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
            Assert.That(tokenResult.Scopes.Count(), Is.EqualTo(1));
            Assert.That(tokenResult.Scopes.Select(s => s), Is.EquivalentTo(new [] { "projectx.rest" }));
        }
        
        #endregion
    }
}