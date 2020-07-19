using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Antibody.CareToKnowPro.CRM.Models;
using Microsoft.AspNetCore.Authorization;

namespace Antibody.CareToKnowPro.CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSpecialtiesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public UserSpecialtiesController(DbAntibodyCareToKnowProContext context, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
        }

        // GET: api/UserSpecialties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSpecialty>>> GetUserSpecialty()
        {
            return await _context.UserSpecialty.ToListAsync();
        }

        // GET: api/UserSpecialties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSpecialty>> GetUserSpecialty(int id)
        {
            var userSpecialty = await _context.UserSpecialty.FindAsync(id);

            if (userSpecialty == null)
            {
                return NotFound();
            }

            return userSpecialty;
        }

        // PUT: api/UserSpecialties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSpecialty(int id, UserSpecialty userSpecialty)
        {
            if (id != userSpecialty.UserSpecialtyId)
            {
                return BadRequest();
            }

            _context.Entry(userSpecialty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSpecialtyExists(id))
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

        // POST: api/UserSpecialties
        [HttpPost]
        public async Task<ActionResult<UserSpecialty>> PostUserSpecialty(UserSpecialty userSpecialty)
        {
            _context.UserSpecialty.Add(userSpecialty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserSpecialty", new { id = userSpecialty.UserSpecialtyId }, userSpecialty);
        }

        // DELETE: api/UserSpecialties/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserSpecialty>> DeleteUserSpecialty(int id)
        {
            var userSpecialty = await _context.UserSpecialty.FindAsync(id);
            if (userSpecialty == null)
            {
                return NotFound();
            }

            _context.UserSpecialty.Remove(userSpecialty);
            await _context.SaveChangesAsync();

            return userSpecialty;
        }

        private bool UserSpecialtyExists(int id)
        {
            return _context.UserSpecialty.Any(e => e.UserSpecialtyId == id);
        }
    }
}
