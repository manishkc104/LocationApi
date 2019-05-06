using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetUpWebApi.Models;

namespace MeetUpWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergenciesController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public EmergenciesController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/Emergencies
        [HttpGet]
        public IEnumerable<Emergency> GetEmergency()
        {
            return _context.Emergency;
        }

        // GET: api/Emergencies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmergency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emergency = await _context.Emergency.FindAsync(id);

            if (emergency == null)
            {
                return NotFound();
            }

            return Ok(emergency);
        }

        // PUT: api/Emergencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmergency([FromRoute] int id, [FromBody] Emergency emergency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emergency.EmergencyID)
            {
                return BadRequest();
            }

            _context.Entry(emergency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmergencyExists(id))
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

        // POST: api/Emergencies
        [HttpPost]
        public async Task<IActionResult> PostEmergency([FromBody] Emergency emergency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Emergency.Add(emergency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergency", new { id = emergency.EmergencyID }, emergency);
        }

        // DELETE: api/Emergencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emergency = await _context.Emergency.FindAsync(id);
            if (emergency == null)
            {
                return NotFound();
            }

            _context.Emergency.Remove(emergency);
            await _context.SaveChangesAsync();

            return Ok(emergency);
        }

        private bool EmergencyExists(int id)
        {
            return _context.Emergency.Any(e => e.EmergencyID == id);
        }
    }
}