using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using ProjectX.Data.EntityFrameworkCore.Scope;
using ProjectX.Data.EntityFrameworkCore.Version;
using ProjectX.Data.Version;
using ProjectX.Testing;

namespace ProjectX.Api.Tests
{
    public class VersionServiceTests : TestBase
    {
        [Test]
        public void Get_ThrowsException_WhenNoVersion()
        {
            //Arrange
            var service = new VersionService(CreateDbContextScopeFactory(), new VersionRepository(new AmbientDbContextLocator()));

            //Act
            async Task Act()
            {
                await service.Get();
            }

            //Assert
            var exception = Assert.ThrowsAsync<AmbiguousMatchException>(Act);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("No version found"));
        }

        [Test]
        public async Task Get_ThrowsException_WhenMultipleVersions()
        {
            //Arrange
            await DbContextForArrange.Versions.AddAsync(new EntityVersion { Major = 1, Minor = 2, Build = 3, Revision = 4 });
            await DbContextForArrange.Versions.AddAsync(new EntityVersion { Major = 5, Minor = 6, Build = 7, Revision = 8 });
            await DbContextForArrange.SaveChangesAsync();

            var service = new VersionService(CreateDbContextScopeFactory(), new VersionRepository(new AmbientDbContextLocator()));

            //Act
            async Task Act()
            {
                await service.Get();
            }

            //Assert
            var exception = Assert.ThrowsAsync<AmbiguousMatchException>(Act);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Multiple versions found"));
        }

        [Test]
        public async Task Get_ReturnsVersion()
        {
            //Arrange
            await DbContextForArrange.Versions.AddAsync(new EntityVersion { Major = 1, Minor = 2, Build = 3, Revision = 4 });
            await DbContextForArrange.SaveChangesAsync();

            var service = new VersionService(CreateDbContextScopeFactory(), new VersionRepository(new AmbientDbContextLocator()));

            //Act
            var version = await service.Get();

            //Assert
            Assert.That(version, Is.Not.Null);
            Assert.That(version.Major, Is.EqualTo(1));
            Assert.That(version.Minor, Is.EqualTo(2));
            Assert.That(version.Build, Is.EqualTo(3));
            Assert.That(version.Revision, Is.EqualTo(4));

            var entityVersion = DbContextForAssert.Versions.Single();
            Assert.That(entityVersion.CreatedDateTime, Is.EqualTo(DateTimeOffset.UtcNow).Within(1).Seconds);
            Assert.That(entityVersion.UpdatedDateTime, Is.EqualTo(DateTimeOffset.UtcNow).Within(1).Seconds);
        }
    }
}