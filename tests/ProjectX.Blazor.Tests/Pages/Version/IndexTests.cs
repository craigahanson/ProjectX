using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using ProjectX.Api;
using ProjectX.Blazor.Pages.Version;
using ProjectX.Blazor.Services;
using TestContext = Bunit.TestContext;

namespace ProjectX.Blazor.Tests.Pages.Version
{
    public class IndexTests
    {
        [Test]
        public void Index_DisplaysLoading_WhenVersionIsNull()
        {
            //Arrange
            var versionService = Substitute.For<IBlazorVersionService>();
            versionService.GetAsync().Returns((ApiVersion) null);

            using var context = new TestContext();
            context.Services.Add(ServiceDescriptor.Singleton(versionService));

            //Act
            var index = context.RenderComponent<Index>();

            //Assert
            var h1 = index.Find("h1");
            Assert.That(h1, Is.Not.Null);
            Assert.That(h1.TextContent, Is.EqualTo("Version"));

            var p = index.Find("p");
            Assert.That(p, Is.Not.Null);
            Assert.That(p.TextContent, Is.EqualTo("Loading..."));
        }

        [Test]
        public void Index_DisplaysVersion_WhenVersionIsSet()
        {
            //Arrange
            var versionService = Substitute.For<IBlazorVersionService>();
            versionService.GetAsync().Returns(new ApiVersion { Major = 1, Minor = 2, Build = 3, Revision = 4 });

            using var context = new TestContext();
            context.Services.Add(ServiceDescriptor.Singleton(versionService));

            //Act
            var index = context.RenderComponent<Index>();

            //Assert
            var h1 = index.Find("h1");
            Assert.That(h1, Is.Not.Null);
            Assert.That(h1.TextContent, Is.EqualTo("Version"));

            var div = index.Find("div");
            Assert.That(div, Is.Not.Null);
            Assert.That(div.TextContent, Is.EqualTo("1.2.3.4"));
        }
    }
}