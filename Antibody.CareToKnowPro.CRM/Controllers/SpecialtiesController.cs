using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Antibody.CareToKnowPro.CRM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Antibody.CareToKnowPro.CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public SpecialtiesController(DbAntibodyCareToKnowProContext context)
        {
            _context = context;
        }

        // GET: api/Specialties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialty>>> GetSpecialty()
        {
            return await _context.Specialty.ToListAsync();
        }

        // GET: api/Specialties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialty>> GetSpecialty(int id)
        {
            var specialty = await _context.Specialty.FindAsync(id);

            if (specialty == null)
            {
                return NotFound();
            }

            return specialty;
        }

        // PUT: api/Specialties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialty(int id, Specialty specialty)
        {
            if (id != specialty.SpecialtyId)
            {
                return BadRequest();
            }

            _context.Entry(specialty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialtyExists(id))
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

        // POST: api/Specialties
        [HttpPost]
        public async Task<ActionResult<Specialty>> PostSpecialty(Specialty specialty)
        {
            _context.Specialty.Add(specialty);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpecialtyExists(specialty.SpecialtyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSpecialty", new { id = specialty.SpecialtyId }, specialty);
        }

        // DELETE: api/Specialties/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Specialty>> DeleteSpecialty(int id)
        {
            var specialty = await _context.Specialty.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }

            _context.Specialty.Remove(specialty);
            await _context.SaveChangesAsync();

            return specialty;
        }

        private bool SpecialtyExists(int id)
        {
            return _context.Specialty.Any(e => e.SpecialtyId == id);
        }
    }
}
