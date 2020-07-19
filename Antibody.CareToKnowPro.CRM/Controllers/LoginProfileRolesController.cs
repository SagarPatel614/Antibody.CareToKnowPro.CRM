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
    public class LoginProfileRolesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public LoginProfileRolesController(DbAntibodyCareToKnowProContext context, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
        }

        // GET: api/LoginProfileRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginProfileRole>>> GetLoginProfileRole()
        {
            return await _context.LoginProfileRole.ToListAsync();
        }

        // GET: api/LoginProfileRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginProfileRole>> GetLoginProfileRole(int id)
        {
            var loginProfileRole = await _context.LoginProfileRole.FindAsync(id);

            if (loginProfileRole == null)
            {
                return NotFound();
            }

            return loginProfileRole;
        }

        // PUT: api/LoginProfileRoles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginProfileRole(int id, LoginProfileRole loginProfileRole)
        {
            if (id != loginProfileRole.LoginProfileRoleId)
            {
                return BadRequest();
            }

            _context.Entry(loginProfileRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginProfileRoleExists(id))
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

        // POST: api/LoginProfileRoles
        [HttpPost]
        public async Task<ActionResult<LoginProfileRole>> PostLoginProfileRole(LoginProfileRole loginProfileRole)
        {
            _context.LoginProfileRole.Add(loginProfileRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoginProfileRole", new { id = loginProfileRole.LoginProfileRoleId }, loginProfileRole);
        }

        // DELETE: api/LoginProfileRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoginProfileRole>> DeleteLoginProfileRole(int id)
        {
            var loginProfileRole = await _context.LoginProfileRole.FindAsync(id);
            if (loginProfileRole == null)
            {
                return NotFound();
            }

            _context.LoginProfileRole.Remove(loginProfileRole);
            await _context.SaveChangesAsync();

            return loginProfileRole;
        }

        private bool LoginProfileRoleExists(int id)
        {
            return _context.LoginProfileRole.Any(e => e.LoginProfileRoleId == id);
        }
    }
}
