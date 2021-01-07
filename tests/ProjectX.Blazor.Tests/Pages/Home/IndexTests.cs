using Bunit;
using NUnit.Framework;
using ProjectX.Blazor.Pages.Home;

namespace ProjectX.Blazor.Tests.Pages.Home
{
    public class IndexTests
    {
        [Test]
        public void Index_DisplaysCorrectly()
        {
            //Arrange
            using var context = new Bunit.TestContext();

            //Act
            var index = context.RenderComponent<Index>();

            //Assert
            var h1 = index.Find("h1");
            Assert.That(h1, Is.Not.Null);
            Assert.That(h1.TextContent, Is.EqualTo("Hello, world!"));
        }
    }
}