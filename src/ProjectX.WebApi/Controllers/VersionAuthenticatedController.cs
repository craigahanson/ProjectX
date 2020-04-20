using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Api.Abstractions;

namespace ProjectX.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VersionAuthenticatedController : ControllerBase
    {
        private readonly IVersionService versionService;

        public VersionAuthenticatedController(IVersionService versionService)
        {
            this.versionService = versionService;
        }

        [HttpGet]
        public ApiVersion Get()
        {
            return versionService.Get();
        }
    }
}
