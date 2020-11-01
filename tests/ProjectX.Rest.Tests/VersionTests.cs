using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using ProjectX.Rest;
using ProjectX.Api.Abstractions;

namespace ProjectX.Rest.Tests
{
    public class VersionTests
    {
        public HttpClient Client { get; set; }

        [SetUp]
        public void Setup()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            Client = webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task Get_ReturnsVersion()
        {
            //Act
            var response = await Client.GetAsync("api/version");

            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var version = JsonSerializer.Deserialize<ApiVersion>(response.Content.ReadAsStringAsync().Result, options);
            Assert.That(version, Is.Not.Null);
            Assert.That(version.Major, Is.EqualTo(1));
            Assert.That(version.Minor, Is.EqualTo(0));
            Assert.That(version.Build, Is.EqualTo(0));
            Assert.That(version.Revision, Is.EqualTo(0));
        }
    }
}