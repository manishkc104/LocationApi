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
    public class UserLocationsController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public UserLocationsController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/UserLocations
        [HttpGet]
        public IEnumerable<UserLocation> GetUserLocation()
        {
            return _context.UserLocation;
        }

        // GET: api/UserLocations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userLocation = await _context.UserLocation.FindAsync(id);

            if (userLocation == null)
            {
                return NotFound();
            }

            return Ok(userLocation);
        }

        //Custom Http Get using userID
        //Get location details using userID
        [HttpGet]
        [Route("~/api/getLocation")]
        public IActionResult getLocation(int userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locationDetails = (from ul in _context.UserLocation                                 
                                   where userID == ul.UserID
                                   select new
                                   {
                                       ul.LocationID                                   
                                   });

            if (locationDetails!= null)
            {
                var result = (from r in locationDetails
                              join l in _context.Locations
                              on r.LocationID equals l.LocationID
                              select new
                              {
                                  l.LocationID,
                                  l.LocationTitle,
                                  l.LocationDesc,
                                  l.Latitude,
                                  l.Longitude,
                                  l.MeetupDate,
                                  l.MeetupTime
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

        // PUT: api/UserLocations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLocation([FromRoute] int id, [FromBody] UserLocation userLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userLocation.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLocationExists(id))
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

        // POST: api/UserLocations
        [HttpPost]
        public async Task<IActionResult> PostUserLocation([FromBody] UserLocation userLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserLocation.Add(userLocation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserLocationExists(userLocation.UserID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserLocation", new { id = userLocation.UserID }, userLocation);
        }

        // DELETE: api/UserLocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userLocation = await _context.UserLocation.FindAsync(id);
            if (userLocation == null)
            {
                return NotFound();
            }

            _context.UserLocation.Remove(userLocation);
            await _context.SaveChangesAsync();

            return Ok(userLocation);
        }

        private bool UserLocationExists(int id)
        {
            return _context.UserLocation.Any(e => e.UserID == id);
        }
    }
}