using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ProjectX.Api.Tests
{
    public class PeopleServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAll_ReturnsPeople()
        {
            //Arrange
            var service = new PeopleService();

            //Act
            var people = service.GetAll().ToList();

            //Assert
            Assert.That(people.Count, Is.EqualTo(2));
            Assert.That(people[0].Id, Is.EqualTo(1));
            Assert.That(people[0].FirstName, Is.EqualTo("Craig"));
            Assert.That(people[0].LastName, Is.EqualTo("Hanson"));
            Assert.That(people[1].Id, Is.EqualTo(2));
            Assert.That(people[1].FirstName, Is.EqualTo("Heather"));
            Assert.That(people[1].LastName, Is.EqualTo("Phillips"));
        }

        [Test]
        public void GetById_ReturnsPerson_IfExists()
        {
            //Arrange
            var service = new PeopleService();

            //Act
            var person = service.GetById(1);

            //Assert
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Id, Is.EqualTo(1));
            Assert.That(person.FirstName, Is.EqualTo("Craig"));
            Assert.That(person.LastName, Is.EqualTo("Hanson"));
        }

        [Test]
        public void GetById_ReturnsNull_IfNotExists()
        {
            //Arrange
            var service = new PeopleService();

            //Act
            var person = service.GetById(-1);

            //Assert
            Assert.That(person, Is.Null);
        }
    }
}