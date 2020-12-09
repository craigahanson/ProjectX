using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ProjectX.Rest.Tests
{
    public class TestBase
    {
        public HttpClient HttpClientNoAuth { get; set; }
        public HttpClient HttpClientAuthenticated { get; set; }

        [SetUp]
        public void Setup()
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
                }).CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }
    }
}
