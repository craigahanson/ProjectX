using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using ProjectX.Api;
using ProjectX.Data.Version;

namespace ProjectX.Rest.Tests
{
    public class VersionAuthenticatedTests : RestTestBase
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
            //Arrange
            await DbContextForArrange.Versions.AddAsync(new EntityVersion { Major = 1, Minor = 2, Build = 3, Revision = 4 });
            await DbContextForArrange.SaveChangesAsync();

            //Act
            var response = await HttpClientAuthenticated.GetAsync("api/versionauthenticated");

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var version = JsonSerializer.Deserialize<ApiVersion>(response.Content.ReadAsStringAsync().Result, options);
            Assert.That(version, Is.Not.Null);
            Assert.That(version.Major, Is.EqualTo(1));
            Assert.That(version.Minor, Is.EqualTo(2));
            Assert.That(version.Build, Is.EqualTo(3));
            Assert.That(version.Revision, Is.EqualTo(4));
        }
    }
}