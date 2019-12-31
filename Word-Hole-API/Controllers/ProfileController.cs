using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.DB;
using Word_Hole_API.Models.Profile;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly WordHoleDBContext _context;

        public ProfileController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpGet("user")]
        public IActionResult GetUser([FromQuery] UserGet parameters)
        {
            var user = (from users in _context.Users
                        where users.Username == parameters.Username
                        select users).FirstOrDefault();

            if (user == null)
                return BadRequest(new { error = "User not found" });

            var result = new
            {
                username = user.Username,
                userID = user.Id,
                access = user.Access
            };

            return Ok(result);
        }

        [HttpGet("posts")]
        public IActionResult GetUserPosts([FromQuery] UserPostsGet parameters) {
            var postIDs = (from posts in _context.Posts
                         where posts.Userid == parameters.UserID
                         orderby posts.Id descending
                         select posts.Id).ToList();

            return Ok(postIDs);
        }
    }
}