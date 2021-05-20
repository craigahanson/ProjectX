using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using IdentityModel.Client;
using NUnit.Framework;
using ProjectX.Library;
using ApiScope = Duende.IdentityServer.EntityFramework.Entities.ApiScope;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;

namespace ProjectX.Authentication.Tests
{
    [TestFixture]
    public class ClientTests : AuthenticationTestBase
    {
        private const string TokenEndpoint = "https://server/connect/token";
        private const string UserInfoEndpoint = "https://server/connect/userinfo";
        
        #region Token
        
        [Test]
        public async Task Token_ReturnsBadRequest_WhenGrantTypeIsMissing()
        {
            //Arrange
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret")
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
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "implicit"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret")
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
        public async Task Token_ReturnsBadRequest_WhenClientDoesNotExist()
        {
            //Arrange
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "INVALIDCLIENT"),
                new KeyValuePair<string, string>("client_secret", "secret")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

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
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "NOTSECRET")
            });
            
            //Act
            var httpResponseMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);

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
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret"),
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
        public async Task Token_ReturnsOK_WhenClientExistsAndUsesCorrectSecret()
        {
            //Arrange
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "projectx.rest")
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
            Assert.That(tokenResult.Scope, Is.EqualTo("projectx.rest"));
            Assert.That(tokenResult.Scopes.Count(), Is.EqualTo(1));
            Assert.That(tokenResult.Scopes.Select(s => s), Is.EquivalentTo(new [] { "projectx.rest" }));
        }
        
        #endregion
        
        #region UserInfo
        
        [Test]
        public async Task UserInfo_ReturnsForbidden_ForClient()
        {
            //Arrange
            ConfigurationDbContextForArrange.ApiScopes.Add(new ApiScope { Name = "projectx.rest" });
            ConfigurationDbContextForArrange.Clients.Add(new Client
            {
                ClientId = "testclient",  
                AllowedGrantTypes = new List<ClientGrantType> { new ClientGrantType { GrantType = "client_credentials" } },
                AllowedScopes = new List<ClientScope> { new ClientScope { Scope = "projectx.rest" } },
                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }}
            });
            await ConfigurationDbContextForArrange.SaveChangesAsync();
            
            var httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            { 
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "testclient"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("scope", "projectx.rest")
            });
            
            var loginMessage = await HttpClient.PostAsync(TokenEndpoint, httpContent);
            var tokenResult = (await loginMessage.Content.ReadAsStringAsync()).FromJsonAsync<TokenResult>();
            
            //Act
            HttpClient.SetToken("Bearer", tokenResult.AccessToken);
            var httpResponseMessage = await HttpClient.GetAsync(UserInfoEndpoint);

            //Assert
            Assert.That(httpResponseMessage, Is.Not.Null);
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        
        #endregion
    }
}