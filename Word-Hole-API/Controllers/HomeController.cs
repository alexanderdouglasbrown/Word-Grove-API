using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.Home;
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

        [HttpGet("posts")]
        public IActionResult GetPostIDs([FromQuery] PostsGet parameters)
        {
            int lastID = parameters.LastID ?? int.MaxValue;

            var postIDs = (from posts in _context.Posts
                         where posts.Id < lastID
                         orderby posts.Id descending
                         select posts.Id).Take(20).ToList();

            return Ok(postIDs);
        }

        [Authorize]
        [HttpPost("post")]
        public IActionResult SubmitPost([FromBody] PostPost post)
        {
            if (post.Post.Count() > _maxPostCharacterCount)
            {
                return BadRequest();
            }

            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);

            var newPost = new Posts()
            {
                Createdon = DateTime.Now,
                Userid = userID,
                Post = post.Post
            };

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return Ok();
        }

    }
}