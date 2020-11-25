using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ProjectX.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("HttpClient", client => client.BaseAddress = new Uri("https://localhost:1002/api/"))
                            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                                .ConfigureHandler(new [] { "https://localhost:1001" }, scopes: new[] { "opendid", "profile", "projectx.rest" }));

            builder.Services.AddHttpClient("HttpClientNoAuth", client => client.BaseAddress = new Uri("https://localhost:1002/api/"));

            builder.Services.AddOidcAuthentication(options => 
            {
                options.ProviderOptions.Authority = "https://localhost:1001";
                options.ProviderOptions.ClientId = "projectx.blazor";
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("projectx.rest");
                options.ProviderOptions.PostLogoutRedirectUri = "https://localhost:1003";
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddSingleton<Services.IVersionService, Services.VersionService>();
            builder.Services.AddSingleton<Services.IPeopleService, Services.PeopleService>();

            await builder.Build().RunAsync();
        }
    }
}
