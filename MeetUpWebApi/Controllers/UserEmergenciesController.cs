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
    public class UserEmergenciesController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public UserEmergenciesController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/UserEmergencies
        [HttpGet]
        public IEnumerable<UserEmergency> GetUserEmergency()
        {
            return _context.UserEmergency;
        }

        // GET: api/UserEmergencies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserEmergency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmergency = await _context.UserEmergency.FindAsync(id);

            if (userEmergency == null)
            {
                return NotFound();
            }

            return Ok(userEmergency);
        }

        //Custom Http Get using userID
        //Get Emergency details using userID
        [HttpGet]
        [Route("~/api/getEmergency")]
        public IActionResult getEmergency(int userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emergencyDetails = (from ue in _context.UserEmergency
                                   where userID == ue.UserID
                                   select new
                                   {
                                       ue.EmergencyID
                                   });

            if (emergencyDetails != null)
            {
                var result = (from ed in emergencyDetails
                              join e in _context.Emergency
                              on ed.EmergencyID equals e.EmergencyID
                              select new
                              {
                                  e.EmergencyID,
                                  e.EmergencyName,
                                  e.Latitude,
                                  e.Longitude
                              }
                    );

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/UserEmergencies/5
        [HttpPut("{id}")]

        public async Task<IActionResult> PutUserEmergency([FromRoute] int id, [FromBody] UserEmergency userEmergency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userEmergency.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userEmergency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEmergencyExists(id))
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

        // POST: api/UserEmergencies
        [HttpPost]
        public async Task<IActionResult> PostUserEmergency([FromBody] UserEmergency userEmergency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserEmergency.Add(userEmergency);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserEmergencyExists(userEmergency.UserID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserEmergency", new { id = userEmergency.UserID }, userEmergency);
        }

        // DELETE: api/UserEmergencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserEmergency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmergency = await _context.UserEmergency.FindAsync(id);
            if (userEmergency == null)
            {
                return NotFound();
            }

            _context.UserEmergency.Remove(userEmergency);
            await _context.SaveChangesAsync();

            return Ok(userEmergency);
        }

        private bool UserEmergencyExists(int id)
        {
            return _context.UserEmergency.Any(e => e.UserID == id);
        }
    }
}