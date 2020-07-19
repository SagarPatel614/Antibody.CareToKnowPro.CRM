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
    public class UserUnsubscribesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public UserUnsubscribesController(DbAntibodyCareToKnowProContext context, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
        }

        // GET: api/UserUnsubscribes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserUnsubscribe>>> GetUserUnsubscribe()
        {
            return await _context.UserUnsubscribe.ToListAsync();
        }

        // GET: api/UserUnsubscribes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserUnsubscribe>> GetUserUnsubscribe(int id)
        {
            var userUnsubscribe = await _context.UserUnsubscribe.FindAsync(id);

            if (userUnsubscribe == null)
            {
                return NotFound();
            }

            return userUnsubscribe;
        }

        // PUT: api/UserUnsubscribes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserUnsubscribe(int id, UserUnsubscribe userUnsubscribe)
        {
            if (id != userUnsubscribe.UnsubscribeId)
            {
                return BadRequest();
            }

            _context.Entry(userUnsubscribe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserUnsubscribeExists(id))
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

        // POST: api/UserUnsubscribes
        [HttpPost]
        public async Task<ActionResult<UserUnsubscribe>> PostUserUnsubscribe(UserUnsubscribe userUnsubscribe)
        {
            _context.UserUnsubscribe.Add(userUnsubscribe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserUnsubscribe", new { id = userUnsubscribe.UnsubscribeId }, userUnsubscribe);
        }

        // DELETE: api/UserUnsubscribes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserUnsubscribe>> DeleteUserUnsubscribe(int id)
        {
            var userUnsubscribe = await _context.UserUnsubscribe.FindAsync(id);
            if (userUnsubscribe == null)
            {
                return NotFound();
            }

            _context.UserUnsubscribe.Remove(userUnsubscribe);
            await _context.SaveChangesAsync();

            return userUnsubscribe;
        }

        private bool UserUnsubscribeExists(int id)
        {
            return _context.UserUnsubscribe.Any(e => e.UnsubscribeId == id);
        }
    }
}
