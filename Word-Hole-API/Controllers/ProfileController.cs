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
            var query = (from posts in _context.Posts
                         join users in _context.Users on posts.Userid equals users.Id
                         where posts.Userid == parameters.UserID
                         orderby posts.Id descending
                         select new { posts, users });

            var result = new List<object>();

            foreach (var post in query)
            {
                var row = new
                {
                    id = post.posts.Id,
                    post = post.posts.Post,
                    date = post.posts.Createdon.ToString(),
                    username = post.users.Username // ¯\_(ツ)_/¯
                };

                result.Add(row);
            }

            return Ok(result);
        }
    }
}