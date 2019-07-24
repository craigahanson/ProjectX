using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Api.Abstractions;

namespace ProjectX.WebApi.Controllers
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
        public ApiVersion Get()
        {
            return versionService.Get();
        }
    }
}
