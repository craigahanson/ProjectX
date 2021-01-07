using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ProjectX.Testing;

namespace ProjectX.Rest.Tests
{
    public class RestTestBase : TestBase
    {
        public HttpClient HttpClientNoAuth { get; set; }
        public HttpClient HttpClientAuthenticated { get; set; }

        [SetUp]
        public new void Setup()
        {
            HttpClientNoAuth = new WebApplicationFactory<Startup>()
                .CreateClient(new WebApplicationFactoryClientOptions());

            HttpClientAuthenticated = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<JwtBearerOptions, TestAuthHandler>(
                                "Test", options => { });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }
    }
}