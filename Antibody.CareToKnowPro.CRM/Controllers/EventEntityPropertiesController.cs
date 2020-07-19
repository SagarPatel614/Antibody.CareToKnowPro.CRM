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
    public class EventEntityPropertiesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public EventEntityPropertiesController(DbAntibodyCareToKnowProContext context, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
        }

        // GET: api/EventEntityProperties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventEntityProperty>>> GetEventEntityProperty()
        {
            return await _context.EventEntityProperty.ToListAsync();
        }

        // GET: api/EventEntityProperties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventEntityProperty>> GetEventEntityProperty(long id)
        {
            var eventEntityProperty = await _context.EventEntityProperty.FindAsync(id);

            if (eventEntityProperty == null)
            {
                return NotFound();
            }

            return eventEntityProperty;
        }

        // PUT: api/EventEntityProperties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventEntityProperty(long id, EventEntityProperty eventEntityProperty)
        {
            if (id != eventEntityProperty.EventEntityPropertyId)
            {
                return BadRequest();
            }

            _context.Entry(eventEntityProperty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventEntityPropertyExists(id))
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

        // POST: api/EventEntityProperties
        [HttpPost]
        public async Task<ActionResult<EventEntityProperty>> PostEventEntityProperty(EventEntityProperty eventEntityProperty)
        {
            _context.EventEntityProperty.Add(eventEntityProperty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventEntityProperty", new { id = eventEntityProperty.EventEntityPropertyId }, eventEntityProperty);
        }

        // DELETE: api/EventEntityProperties/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventEntityProperty>> DeleteEventEntityProperty(long id)
        {
            var eventEntityProperty = await _context.EventEntityProperty.FindAsync(id);
            if (eventEntityProperty == null)
            {
                return NotFound();
            }

            _context.EventEntityProperty.Remove(eventEntityProperty);
            await _context.SaveChangesAsync();

            return eventEntityProperty;
        }

        private bool EventEntityPropertyExists(long id)
        {
            return _context.EventEntityProperty.Any(e => e.EventEntityPropertyId == id);
        }
    }
}
