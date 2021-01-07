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