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

            builder.Services.AddHttpClient("ServerAPI.AuthenticationClient", client => client.BaseAddress = new Uri("http://localhost:1002/api/"))
                            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                                                           .ConfigureHandler(new [] { "http://localhost:1001" }, scopes: new[] { "opendid", "profile", "projectx.rest" }));

            builder.Services.AddHttpClient("ServerAPI.NoAuthenticationClient", client => client.BaseAddress = new Uri("http://localhost:1002/api/"));

            builder.Services.AddOidcAuthentication(options => 
            {
                options.ProviderOptions.Authority = "http://localhost:1001";
                options.ProviderOptions.ClientId = "projectx.blazor";
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("projectx.rest");
                options.ProviderOptions.PostLogoutRedirectUri = "http://localhost:1003";
                options.ProviderOptions.ResponseType = "code";
            });

            await builder.Build().RunAsync();
        }
    }
}
