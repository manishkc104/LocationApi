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
    public class UserRelationsController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public UserRelationsController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/UserRelations
        [HttpGet]
        public IEnumerable<UserRelation> GetUserRelation()
        {
            return _context.UserRelation;
        }

        // GET: api/UserRelations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRelation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRelation = await _context.UserRelation.FindAsync(id);

            if (userRelation == null)
            {
                return NotFound();
            }

            return Ok(userRelation);
        }

        //Get all users that are friend and get all users that have pending request.
        [HttpGet]
        [Route("~/api/getFriendList")]
        public IActionResult getFriendList(int userA, string statusA)
        {
            List<Users> usr = new List<Users>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = (from r in _context.UserRelation                          
                           where (r.UserA == userA || r.UserB == userA) && (r.StatusA == statusA || r.StatusB == statusA)
                           select r
                          ).ToList();

            foreach (var item in request)
            {
                if (userA == item.UserA)
                {
                    var result1 = (from u in _context.Users
                                   where u.UserID == item.UserB
                                   select u
                                   
                        ).SingleOrDefault();
                    usr.Add(result1);
                }

                else
                {
                    var result2 = (from u in _context.Users
                                   where u.UserID == item.UserA
                                   select 
                                   u
                          ).SingleOrDefault();
                    usr.Add(result2);

                }

            }
            return Ok(usr);

        }

        //Get all users that are not friend.
        [HttpGet]
        [Route("~/api/notFriendList/{id}")]
        public IActionResult notFriendList( int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else {
                return NotFound();
            }
        }

        //POST API, send friend request to users.
        [HttpPost]
        [Route("~/api/sendRequest")]
        public IActionResult sendRequest(int userA, int userB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserRelation urs = new UserRelation();
            urs.UserA = userA;
            urs.UserB = userB;
            urs.StatusA = "Pending";
            urs.StatusB = "Received";

            _context.UserRelation.Add(urs);
            _context.SaveChangesAsync();

            return Ok();
        }


        //Get friend request.
        [HttpGet]
        [Route("~/api/getRequest")]
        public IActionResult getRequest(int userB)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = (from r in _context.UserRelation
                                    where r.UserB == userB && r.StatusB == "Received"
                                    select r
                                    );
            if (request != null)
            {
                var result = (from r in  request
                              join u in _context.Users
                              on r.UserA equals u.UserID
                              select new
                              {
                                  u.UserID,
                                  u.UserName
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
            //var result = _context.UserRelation;

            //return Ok(result);
            else {
                return NotFound();
            }
        }

       
        //PUT API, Accept friend request.
        [HttpPut]
        [Route("~/api/acceptRequest")]
        public IActionResult acceptRequest(int userA, int userB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRelation result = (from r in _context.UserRelation
                                   where r.UserA == userA && r.UserB == userB
                                   select r).SingleOrDefault();


            if (result != null)
            {
                result.StatusA = "Friends";
                result.StatusB = "Friends";

                _context.UserRelation.Update(result);
                _context.SaveChanges();
                return Ok();
            }
            else {
                return NotFound();
            }
                                               

        }

        //PUT API, Decline friend request or delete request.
        [HttpDelete]
        [Route("~/api/declineRequest")]
        public IActionResult declineRequest(int userA, int userB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRelation result = (from r in _context.UserRelation
                                   where r.UserA == userA && r.UserB == userB
                                   select r).SingleOrDefault();


            if (result != null)
            {
                _context.UserRelation.Remove(result);
                _context.SaveChangesAsync();

                return Ok();

                
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/UserRelations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRelation([FromRoute] int id, [FromBody] UserRelation userRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userRelation.UserA)
            {
                return BadRequest();
            }

            _context.Entry(userRelation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRelationExists(id))
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

        // POST: api/UserRelations
        [HttpPost]
        public async Task<IActionResult> PostUserRelation([FromBody] UserRelation userRelation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserRelation.Add(userRelation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserRelationExists(userRelation.UserA))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserRelation", new { id = userRelation.UserA }, userRelation);
        }

        // DELETE: api/UserRelations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRelation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRelation = await _context.UserRelation.FindAsync(id);
            if (userRelation == null)
            {
                return NotFound();
            }

            _context.UserRelation.Remove(userRelation);
            await _context.SaveChangesAsync();

            return Ok(userRelation);
        }

        private bool UserRelationExists(int id)
        {
            return _context.UserRelation.Any(e => e.UserA == id);
        }
    }
}