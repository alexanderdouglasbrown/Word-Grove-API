using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.DB;
using Word_Hole_API.Models.Post;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly WordHoleDBContext _context;

        public PostController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPost([FromQuery] PostGet parameters)
        {
            var postQuery = (from posts in _context.Posts
                             join users in _context.Users on posts.Userid equals users.Id
                             where posts.Id == parameters.ID
                             select new { posts, users}).FirstOrDefault();

            if (postQuery == null)
                return BadRequest(new { error = "No post found" });

            var post = new
            {
                post = postQuery.posts.Post,
                date = postQuery.posts.Createdon.ToString(),
                username = postQuery.users.Username
            };

            var comments = (from cmts in _context.Comments
                            where cmts.Postid == parameters.ID
                            select cmts).ToList();

            return Ok(new { post, comments });
        }
    }
}