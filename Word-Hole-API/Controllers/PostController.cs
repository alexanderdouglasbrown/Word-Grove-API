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
            var userID = GetNullableUserID();

            var postQuery = (from posts in _context.Posts
                             join users in _context.Users on posts.Userid equals users.Id
                             where posts.Id == parameters.ID
                             select new { posts, users }).FirstOrDefault();

            if (postQuery == null)
                return BadRequest(new { notFound = true });

            var likesQuery = from likes in _context.Likes
                             where likes.Postid == parameters.ID
                             select likes;

            var isUserLiked = GetUserLikedPost(userID, likesQuery);
            var totalLikes = likesQuery.Count();

            var totalComments = (from comments in _context.Comments
                                 where comments.Postid == parameters.ID
                                 select comments).Count();

            // 5 minute leeway before a post is considered edited
            var isEdited = postQuery.posts.Editdate.HasValue && postQuery.posts.Editdate > postQuery.posts.Createdon.AddMinutes(5);

            var post = new
            {
                post = postQuery.posts.Post,
                date = postQuery.posts.Createdon.ToString("dddd, MMM dd, yyyy hh:mm tt"),
                isEdited,
                username = postQuery.users.Username,
                userID = postQuery.users.Id
            };

            return Ok(new { post, totalLikes, totalComments, isUserLiked });
        }

        private int? GetNullableUserID()
        {
            var userIDRaw = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID");
            int? result = null;

            if (userIDRaw != null)
                result = int.Parse(userIDRaw.Value);

            return result;
        }

        private bool GetUserLikedPost(int? userID, IQueryable<Likes> likesQuery)
        {
            if (!userID.HasValue)
                return false;

            var getLike = (from likes in likesQuery
                           where likes.Userid == userID
                           select likes).FirstOrDefault();

            return getLike != null;
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