using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Api.Abstractions;

namespace ProjectX.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            this.peopleService = peopleService;
        }

        [HttpGet]
        public IEnumerable<ApiPerson> Get()
        {
            return peopleService.GetAll();
        }

        [HttpGet("{id}")]
        public ApiPerson Get(int id)
        {
            return peopleService.GetById(id);
        }
    }
}
