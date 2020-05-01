using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options => 
            {
                options.ProviderOptions.Authority = "https://localhost:1001";
                options.ProviderOptions.ClientId = "projectx.blazor";
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("projectx.webapi");
                options.ProviderOptions.PostLogoutRedirectUri = "https://localhost:1003";
                options.ProviderOptions.ResponseType = "code";
            });

            await builder.Build().RunAsync();
        }
    }
}
