using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Api;

namespace ProjectX.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionAuthenticatedController : ControllerBase
    {
        private readonly IVersionService versionService;

        public VersionAuthenticatedController(IVersionService versionService)
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