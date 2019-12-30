using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private const int _maxPostCharacterCount = 512;

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
                             select new { posts, users }).FirstOrDefault();

            if (postQuery == null)
                return BadRequest(new { error = "No post found" });

            // 5 minute leeway before a post is considered edited
            var isEdited = postQuery.posts.Editdate.HasValue && postQuery.posts.Editdate > postQuery.posts.Createdon.AddMinutes(5);

            var post = new
            {
                post = postQuery.posts.Post,
                date = postQuery.posts.Createdon.ToString(),
                isEdited,
                username = postQuery.users.Username,
                userID = postQuery.users.Id
            };

            return Ok(post);
        }

        [Authorize]
        [HttpPatch]
        public IActionResult EditPost(PostPatch parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);
            var role = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            var post = (from posts in _context.Posts
                        where posts.Id == parameters.ID
                        select posts).Single();

            if (role != "Admin" && post.Userid != userID)
                return BadRequest(new { error = "You do not have permission to edit this post" });

            if (parameters.Post.Count() > _maxPostCharacterCount)
                return BadRequest(new { error = "Your post has too many characters" });

            post.Editdate = DateTime.Now;
            post.Post = parameters.Post;

            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeletePost(PostDelete parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);
            var role = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            var post = (from posts in _context.Posts
                        where posts.Id == parameters.ID
                        select posts).Single();

            if (role != "Admin" && post.Userid != userID)
                return BadRequest(new { error = "You do not have permission to delete this post" });

            var comments = from cmts in _context.Comments
                           where cmts.Postid == parameters.ID
                           select cmts;

            var likes = from lks in _context.Likes
                        where lks.Postid == parameters.ID
                        select lks;

            _context.Likes.RemoveRange(likes);
            _context.Comments.RemoveRange(comments);
            _context.Posts.Remove(post);

            _context.SaveChanges();

            return Ok();
        }
    }
}