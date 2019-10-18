﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Mvc.Models;

using ProjectX.Api.Abstractions;

namespace ProjectX.Mvc.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleService peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            this.peopleService = peopleService;
        }

        [Route("people")]
        public IActionResult Index()
        {
            var people = peopleService.GetAll();

            return View(new PeopleModel
            {
                People = people.Select(p => new PersonModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName
                })
            });
        }

        [Route("people/{id:int}")]
        public IActionResult Person(int id)
        {
            var person = peopleService.GetById(id);
            if(person == null)
            {
                RedirectToAction("Home", "Error");
            }

            return View("Person", new PersonModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            });
        }
    }
}