using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ProjectX.Api.Abstractions;
using System.Net.Http;

namespace ProjectX.Blazor.Pages.Version
{
    public class IndexModel : ComponentBase {
        
        private HttpClient httpClient;
        public ApiVersion version;

        public IndexModel()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:1001/api/");
        }

        protected override async Task OnInitializedAsync()
        {
            version = await httpClient.GetJsonAsync<ApiVersion>("version");
        }
    }
}