using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ProjectX.Api.Abstractions;
using System.Net.Http;

namespace ProjectX.Blazor.Pages.People
{
    public class IndexModel : ComponentBase {
        
        private HttpClient httpClient;
        public ApiPerson[] people;

        public IndexModel()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:1001/api/");
        }

        protected override async Task OnInitializedAsync()
        {
            people = await httpClient.GetJsonAsync<ApiPerson[]>("people");
        }
    }
}