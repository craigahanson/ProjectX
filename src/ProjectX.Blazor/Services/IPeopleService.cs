using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectX.Api.Abstractions;

namespace ProjectX.Blazor.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<ApiPerson>> GetAllAsync();
        Task<ApiPerson> GetAsync(int id);
    }
}