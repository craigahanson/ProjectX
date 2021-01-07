using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectX.Api;

namespace ProjectX.Blazor.Services
{
    public interface IBlazorVersionService
    {
        Task<ApiVersion> GetAsync();
        Task<ApiVersion> GetAuthenticatedAsync();
    }
}