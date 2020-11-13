using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProjectX.Api.Abstractions;

namespace ProjectX.Blazor.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PeopleService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ApiPerson>> GetAllAsync()
        {
            var httpClient = httpClientFactory.CreateClient("HttpClientNoAuth");
            return await httpClient.GetFromJsonAsync<IEnumerable<ApiPerson>>("people");
        }

        public async Task<ApiPerson> GetAsync(int id)
        {
            var httpClient = httpClientFactory.CreateClient("HttpClientNoAuth");
            return await httpClient.GetFromJsonAsync<ApiPerson>($"people/{id}");
        }
    }
}