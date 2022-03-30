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

namespace Phonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubdivisionsController : ControllerBase
    {
        private readonly PhonebookContext _context;

        public SubdivisionsController(PhonebookContext context)
        {
            _context = context;
        }

        // GET: api/Subdivisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subdivision>>> GetSubdivisions()
        {
            return await _context.Subdivisions.ToListAsync();
        }

        // GET: api/Subdivisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subdivision>> GetSubdivision(int id)
        {
            var subdivision = await _context.Subdivisions.FindAsync(id);

            if (subdivision == null)
            {
                return NotFound();
            }

            return subdivision;
        }

        // PUT: api/Subdivisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubdivision(int id, Subdivision subdivision)
        {
            if (id != subdivision.Id)
            {
                return BadRequest();
            }

            _context.Entry(subdivision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubdivisionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Subdivisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subdivision>> PostSubdivision(Subdivision subdivision)
        {
            _context.Subdivisions.Add(subdivision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubdivision", new { id = subdivision.Id }, subdivision);
        }

        // DELETE: api/Subdivisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubdivision(int id)
        {
            var subdivision = await _context.Subdivisions.FindAsync(id);
            if (subdivision == null)
            {
                return NotFound();
            }

            _context.Subdivisions.Remove(subdivision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubdivisionExists(int id)
        {
            return _context.Subdivisions.Any(e => e.Id == id);
        }
    }
}
