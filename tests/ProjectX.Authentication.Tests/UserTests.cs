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
    public class UserTests : AuthenticationTestBase
    {
        private const string TokenEndpoint = "https://server/connect/token";
        private const string UserInfoEndpoint = "https://server/connect/userinfo";
        
        #region Token
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenGrantTypeIsMissing()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "Welcome123")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

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
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "implicit"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "Welcome123")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("unsupported_grant_type"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "INVALIDUSER"),
                new KeyValuePair<string, string>("password", "Welcome123")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_grant"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenPasswordDoesNotMatch()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "NOTPASSWORD")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_grant"));
        }
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenScopeIsInvalid()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "Welcome123"),
                new KeyValuePair<string, string>("scope", "INVALIDSCOPE")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var tokenError = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenError>();
            Assert.That(tokenError.Error, Is.EqualTo("invalid_scope"));
        }
        
        [Test]
        public async Task Token_ReturnsOK_WhenUserExistsAndUsesCorrectPassword()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "Welcome123")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);
            
            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var tokenResult = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenResult>();
            Assert.That(tokenResult, Is.Not.Null);
            Assert.That(tokenResult.AccessToken, Is.Not.Null.Or.Empty);
            Assert.That(tokenResult.TokenType, Is.EqualTo("Bearer"));
            Assert.That(tokenResult.ExpiresIn, Is.EqualTo(3600));
            Assert.That(tokenResult.Scope, Is.EqualTo("openid profile projectx.rest"));
            Assert.That(tokenResult.Scopes.Count(), Is.EqualTo(3));
            Assert.That(tokenResult.Scopes.Select(s => s), Is.EquivalentTo(new [] { "openid", "profile", "projectx.rest" }));
        }
        
        #endregion
        
        #region UserInfo
        
        [Test]
        public async Task UserInfo_ReturnsOk_ForUser()
        {
            //Arrange
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("client_id", "projectx.postman"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "Craig"),
                new KeyValuePair<string, string>("password", "Welcome123")
            });
            var loginMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);
            var tokenResult = (await loginMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenResult>();
            
            //Act
            HttpClient.SetToken("Bearer", tokenResult.AccessToken);
            var httpResponseMessage = await HttpClient.GetAsync(UserInfoEndpoint);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var userInfoResult = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJsonAsync<UserInfoResult>();
            Assert.That(userInfoResult, Is.Not.Null);
            Assert.That(userInfoResult.Name, Is.EqualTo("Craig Hanson"));
            Assert.That(userInfoResult.GivenName, Is.EqualTo("Craig"));
            Assert.That(userInfoResult.FamilyName, Is.EqualTo("Hanson"));
            Assert.That(userInfoResult.Sub, Is.Not.Empty);
        }
        
        #endregion
    }
}