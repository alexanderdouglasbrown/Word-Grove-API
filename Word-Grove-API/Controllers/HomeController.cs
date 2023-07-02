using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Grove_API.Models.Home;
using Word_Grove_API.Models.DB;
using Word_Grove_API.Shared;

namespace Word_Grove_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly WordGroveDBContext _context;
        private const int _maxPostCharacterCount = 512;

        public HomeController(WordGroveDBContext context)
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
            if (post.Post.Length > _maxPostCharacterCount)
            {
                return BadRequest();
            }

            var userID = JWTUtility.GetUserID(HttpContext);

            var newPost = new Posts()
            {
                Createdon = DateTime.Now,
                Userid = userID,
                Post = post.Post,
                Imageurl = post.ImageURL
            };

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return Ok();
        }

    }
}
