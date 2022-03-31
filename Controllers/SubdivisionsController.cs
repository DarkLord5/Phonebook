#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
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
        public async Task<ActionResult<Subdivision>> PutSubdivision(int id, Subdivision subdivision) =>
            await _subdivisionService.UpdateSubdivisionAsync(id, subdivision);

        // POST: api/Subdivisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subdivision>> PostSubdivision(Subdivision subdivision) =>
            await _subdivisionService.CreateSubdivisionAsync(subdivision);

        // DELETE: api/Subdivisions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Subdivision>>> DeleteSubdivision(int id) =>
            await _subdivisionService.DeleteSubdivisionAsync(id);
    }
}
