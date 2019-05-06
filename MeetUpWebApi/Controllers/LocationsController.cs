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
    [Produces("application/json")]
    [Route("api/locations")]

    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public LocationsController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public IEnumerable<Locations> GetLocations()
        {
            return _context.Locations;
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocations([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _context.Locations.FindAsync(id);

            if (locations == null)
            {
                return NotFound();
            }

            return Ok(locations);
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocations([FromRoute] int id, [FromBody] Locations locations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locations.LocationID)
            {
                return BadRequest();
            }

            _context.Entry(locations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationsExists(id))
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

        // POST: api/Locations
        [HttpPost]
        public async Task<IActionResult> PostLocations([FromBody] Locations locations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Locations.Add(locations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocations", new { id = locations.LocationID }, locations);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocations([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _context.Locations.FindAsync(id);
            if (locations == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(locations);
            await _context.SaveChangesAsync();

            return Ok(locations);
        }

        private bool LocationsExists(int id)
        {
            return _context.Locations.Any(e => e.LocationID == id);
        }
    }
}