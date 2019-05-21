using System.Collections.Generic;

namespace ProjectX.Api.Abstractions
{
    public interface IPeopleService
    {
        IEnumerable<ApiPerson> GetAll();
        ApiPerson GetById(int id);
    }
}