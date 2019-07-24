using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ProjectX.Api.Tests
{
    public class VersionServiceTests
    {
        [Test]
        public void Get_ReturnsVersion()
        {
            //Arrange
            var service = new VersionService();

            //Act
            var version = service.Get();

            //Assert
            Assert.That(version, Is.Not.Null);
            Assert.That(version.Major, Is.EqualTo(1));
            Assert.That(version.Minor, Is.EqualTo(0));
            Assert.That(version.Build, Is.EqualTo(0));
            Assert.That(version.Revision, Is.EqualTo(0));
        }
    }
}