using System.Collections.Generic;
using System.Linq;
using ProjectX.Api.Abstractions;

namespace ProjectX.Api
{
    public class PeopleService : IPeopleService
    {
        private ApiPerson[] people => new ApiPerson []
        {
            new ApiPerson { Id = 1, FirstName = "Craig", LastName = "Hanson" },
            new ApiPerson { Id = 2, FirstName = "Heather", LastName = "Phillips" }
        };

        public IEnumerable<ApiPerson> GetAll()
        {
            return people;
        }

        public ApiPerson GetById(int id)
        {
            return people.SingleOrDefault(p => p.Id == id);
        }
    }
}