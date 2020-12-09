using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ProjectX.Rest;
using ProjectX.Api.Abstractions;

namespace ProjectX.Rest.Tests
{
    public class VersionAuthenticatedTests : TestBase
    {
        [Test]
        public async Task Get_ReturnsUnauthorized_WhenNotLoggedIn()
        {
            //Act
            var response = await HttpClientNoAuth.GetAsync("api/versionauthenticated");

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Get_ReturnsVersion_WhenAuthenticated()
        {
            //Act
            var response = await HttpClientAuthenticated.GetAsync("api/versionauthenticated");

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

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