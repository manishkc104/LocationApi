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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MeetUpWebApiContext _context;

        public UsersController(MeetUpWebApiContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        //Facebook user Authentication
        [HttpGet]
        [Route("~/api/GetUserStatus")]
        public IActionResult GetUserStatus(string username, string fbID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            if (!UsersFBidExists(fbID))
                {
                //User Update with fb id
                Users usr = new Users();
                usr.FbID = fbID;
                usr.UserEmail = null;
                usr.UserName = username;
                usr.Password = null;

                _context.Users.Add(usr);
                _context.SaveChangesAsync();

                return CreatedAtAction("GetUsers", new { id = usr.UserID }, usr);
            }
            else
                {
                    var result = (from p in _context.Users
                                  where p.FbID == fbID
                                  select new
                                  {
                                      p.UserID,
                                      p.UserName,
                                      p.UserEmail,
                                      p.Password,
                                      p.FbID
                                  }).SingleOrDefault();
                return Ok(result);
                }

            }

        //User Authentication
        
        [HttpGet]
        [Route("~/api/authUsers")]
        public IActionResult authUsers(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userdetails = (from p in _context.Users
                                           where p.UserName == username
                                           select new
                                           {
                                               p.UserID,
                                               p.UserName,
                                               p.UserEmail,
                                               p.Password
                                           }).SingleOrDefault();
            if(userdetails != null)
            {
                if(userdetails.Password == password)
                {
                    return Ok(userdetails);
                }
                else
                {
                    return Unauthorized();
                }
            }
            
            else
            {
                return NotFound();
            }
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.UserID)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.UserID }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        private bool UsersnameExists(string username)
        {
            var result = from p in _context.Users
                         where p.UserName == username
                         select new
                         {
                             p.UserName
                         };
            if(result != null)
            {
                return true;
            }

            return false;
        }

        private bool UserspasswordExists(string password)
        {
            var result = from p in _context.Users
                         where p.Password == password
                         select new
                         {
                             p.UserName
                         };
            if (result != null)
            {
                return true;
            }

            return false;
        }

        private bool UsersFBidExists(string fbID)
        {
            return _context.Users.Any(e => e.FbID == fbID);
        }



    }
}