using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Blazor.Services;

namespace ProjectX.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("HttpClient", client => client.BaseAddress = new Uri("https://localhost:1002/api/"))
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(new[] { "https://localhost:1001" }, new[] { "opendid", "profile", "projectx.rest" }));

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

            builder.Services.AddScoped<IBlazorVersionService, BlazorVersionService>();

            await builder.Build().RunAsync();
        }
    }
}