using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using ProjectX.Api;

namespace ProjectX.Blazor.Services
{
    public class VersionService : IVersionService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IAccessTokenProvider accessTokenProvider;

        public VersionService(IHttpClientFactory httpClientFactory, IAccessTokenProvider accessTokenProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.accessTokenProvider = accessTokenProvider;
        }

        public async Task<ApiVersion> GetAsync()
        {
            var httpClient = httpClientFactory.CreateClient("HttpClientNoAuth");
            return await httpClient.GetFromJsonAsync<ApiVersion>("version");
        }

        public async Task<ApiVersion> GetAuthenticatedAsync()
        {
            var tokenResult = await accessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions
            {
                Scopes = new[] { "projectx.rest" }
            });

            if(tokenResult.TryGetToken(out var token))
            {
                var httpClient = httpClientFactory.CreateClient("HttpClient");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                return await httpClient.GetFromJsonAsync<ApiVersion>("versionauthenticated");
            }
            
            return null;
        }
    }
}