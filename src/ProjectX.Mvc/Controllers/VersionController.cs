using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Mvc.Models;

using ProjectX.Api.Abstractions;

namespace ProjectX.Mvc.Controllers
{
    public class VersionController : Controller
    {
        private readonly IVersionService versionService;

        public VersionController(IVersionService versionService)
        {
            this.versionService = versionService;
        }

        [Route("Version")]
        public IActionResult Index()
        {
            var version = versionService.Get();

            return View(new VersionModel
            {
                Version = version.ToString()
            });
        }
    }
}