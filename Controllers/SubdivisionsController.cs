#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
using Phonebook.Filters;
using Phonebook.Models;
using Phonebook.Services;

namespace Phonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubdivisionsController : ControllerBase
    {
        private readonly ISubdivisionService _subdivisionService;

        public SubdivisionsController(ISubdivisionService subdivisionService)
        {
            _subdivisionService = subdivisionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Subdivision>>> GetSubdivisions() =>
            await _subdivisionService.GetAllSubdivisionsAsync();


        [HttpPut("{id}")]
        [PhonebookAsyncExceptionFilter]
        public async Task<ActionResult<Subdivision>> UpdateSubdivision(int id, Subdivision subdivision)
        {
            var result = await _subdivisionService.UpdateSubdivisionAsync(id, subdivision);

            if (result == null)
            {
                throw new Exception("There is no such subdivision!");
            }

            return result;
        }

        // POST: api/Subdivisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [PhonebookAsyncExceptionFilter]
        public async Task<ActionResult<Subdivision>> CreateSubdivision(Subdivision subdivision)
        {
            var result = await _subdivisionService.CreateSubdivisionAsync(subdivision);

            if (result == null)
            {
                throw new Exception("This subdivision is empty!");
            }

            return result;
        }

        // DELETE: api/Subdivisions/5
        [HttpDelete("{id}")]
        [PhonebookAsyncExceptionFilter]
        public async Task<ActionResult<List<Subdivision>>> DeleteSubdivision(int id)
        {
            var result = await _subdivisionService.DeleteSubdivisionAsync(id);

            if (result == null)
            {
                throw new Exception("There is no such subdivision!");
            }

            return result;
        }
    }
}
