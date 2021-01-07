using System.Threading.Tasks;

namespace ProjectX.Api.Abstractions
{
    public interface IVersionService
    {
        Task<ApiVersion> Get();
    }
}