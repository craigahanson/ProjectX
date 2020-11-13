using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProjectX.Api.Abstractions;

namespace ProjectX.Blazor.Services
{
    public class VersionService : IVersionService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public VersionService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ApiVersion> GetAsync()
        {
            var httpClient = httpClientFactory.CreateClient("HttpClientNoAuth");
            return await httpClient.GetFromJsonAsync<ApiVersion>("version");
        }
    }
}