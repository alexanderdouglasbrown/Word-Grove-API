using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models;
using Word_Hole_API.Models.DB;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly WordHoleDBContext _context;
        private const int _maxPostCharacterCount = 512;

        public HomeController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpGet("hi")]
        public IActionResult SayHi()
        {
            return Ok();
        }

        [HttpGet("posts")]
        public IActionResult GetPosts()
        {
            var query = (from posts in _context.Posts
                         join users in _context.Users on posts.Userid equals users.Id
                         orderby posts.Createdon descending
                         select new { posts, users }).Skip(0).Take(20).ToList();

            var result = new List<object>();

            foreach(var post in query)
            {
                var row = new
                {
                    id = post.posts.Id,
                    post = post.posts.Post,
                    date = post.posts.Createdon.ToString(),
                    username = post.users.Username
                };

                result.Add(row);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("post")]
        public IActionResult SubmitPost([FromBody] PostBody post)
        {
            if (post.Post.Count() > _maxPostCharacterCount)
            {
                return BadRequest();
            }

            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);

            var newPost = new Posts()
            {
                Createdon = DateTime.UtcNow,
                Userid = userID,
                Post = post.Post
            };

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return Ok();
        }

    }
}