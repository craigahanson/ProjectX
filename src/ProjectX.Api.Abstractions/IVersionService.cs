using System.Threading.Tasks;

namespace ProjectX.Api
{
    public interface IVersionService
    {
        Task<ApiVersion> Get();
    }
}