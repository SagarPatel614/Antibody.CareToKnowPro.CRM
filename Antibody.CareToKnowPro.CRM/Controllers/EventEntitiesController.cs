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
    public class EventEntitiesController : BaseController
    {
        private readonly DbAntibodyCareToKnowProContext _context;

        public EventEntitiesController(DbAntibodyCareToKnowProContext context, DbAntibodyCareToKnowProContext dbContext) : base(dbContext)
        {
            _context = context;
        }

        // GET: api/EventEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventEntity>>> GetEventEntity()
        {
            return await _context.EventEntity.ToListAsync();
        }

        // GET: api/EventEntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventEntity>> GetEventEntity(long id)
        {
            var eventEntity = await _context.EventEntity.FindAsync(id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            return eventEntity;
        }

        // PUT: api/EventEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventEntity(long id, EventEntity eventEntity)
        {
            if (id != eventEntity.EventEntityId)
            {
                return BadRequest();
            }

            _context.Entry(eventEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventEntityExists(id))
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

        // POST: api/EventEntities
        [HttpPost]
        public async Task<ActionResult<EventEntity>> PostEventEntity(EventEntity eventEntity)
        {
            _context.EventEntity.Add(eventEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventEntity", new { id = eventEntity.EventEntityId }, eventEntity);
        }

        // DELETE: api/EventEntities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventEntity>> DeleteEventEntity(long id)
        {
            var eventEntity = await _context.EventEntity.FindAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            _context.EventEntity.Remove(eventEntity);
            await _context.SaveChangesAsync();

            return eventEntity;
        }

        private bool EventEntityExists(long id)
        {
            return _context.EventEntity.Any(e => e.EventEntityId == id);
        }
    }
}
