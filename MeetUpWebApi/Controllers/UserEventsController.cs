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
    public class UserEventsController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public UserEventsController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/UserEvents
        [HttpGet]
        public IEnumerable<UserEvent> GetUserEvent()
        {
            return _context.UserEvent;
        }

        // GET: api/UserEvents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEvent = await _context.UserEvent.FindAsync(id);

            if (userEvent == null)
            {
                return NotFound();
            }

            return Ok(userEvent);
        }

        // PUT: api/UserEvents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserEvent([FromRoute] int id, [FromBody] UserEvent userEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userEvent.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEventExists(id))
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

        //Custom Http Get using userID
        //Get Emergency details using userID
        [HttpGet]
        [Route("~/api/getEvents")]
        public IActionResult getEvents(int userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventDetails = (from ue in _context.UserEvent
                                    where userID == ue.UserID
                                    select new
                                    {
                                        ue.EventID
                                    });

            if (eventDetails != null)
            {
                var result = (from ed in eventDetails
                              join e in _context.Events
                              on ed.EventID equals e.EventID
                              select new
                              {
                                  e.EventID,
                                  e.EventTitle,
                                  e.EventDesc,
                                  e.StartDate,
                                  e.EndDate, 
                                  e.StartTime, 
                                  e.EndTime
                              }
                    );
                if (result != null)
                {
                    return Ok(result);
                }

                else {
                   return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/UserEvents
        [HttpPost]
        public async Task<IActionResult> PostUserEvent([FromBody] UserEvent userEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserEvent.Add(userEvent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserEventExists(userEvent.UserID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserEvent", new { id = userEvent.UserID }, userEvent);
        }

        // DELETE: api/UserEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEvent = await _context.UserEvent.FindAsync(id);
            if (userEvent == null)
            {
                return NotFound();
            }

            _context.UserEvent.Remove(userEvent);
            await _context.SaveChangesAsync();

            return Ok(userEvent);
        }

        private bool UserEventExists(int id)
        {
            return _context.UserEvent.Any(e => e.UserID == id);
        }
    }
}