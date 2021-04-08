using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using ProjectX.Testing;

namespace ProjectX.Authentication.Tests
{
    public class AuthenticationTestBase : TestBase
    {
        public HttpClient HttpClient;
        
        [SetUp]
        public new void Setup()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            HttpClient = server.CreateClient();
        }
    }
}