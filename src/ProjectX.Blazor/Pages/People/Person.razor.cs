using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ProjectX.Api.Abstractions;
using System.Net.Http;

namespace ProjectX.Blazor.Pages.People
{
    public class PersonModel : ComponentBase {
        
        [Parameter] public int Id { get;set; }
        private HttpClient httpClient;
        public ApiPerson person;

        public PersonModel()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:1001/api/");
        }

        protected override async Task OnInitializedAsync()
        {
            person = await httpClient.GetJsonAsync<ApiPerson>($"people/{Id}");
        }
    }
}