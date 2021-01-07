using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Api;

namespace ProjectX.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionService versionService;

        public VersionController(IVersionService versionService)
        {
            this.versionService = versionService;
        }

        [HttpGet]
        public async Task<ApiVersion> Get()
        {
            return await versionService.Get();
        }
    }
}