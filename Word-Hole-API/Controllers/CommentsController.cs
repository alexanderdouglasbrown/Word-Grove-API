using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Hole_API.Models.Comments;
using Word_Hole_API.Models.DB;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly WordHoleDBContext _context;

        public CommentsController(WordHoleDBContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public IActionResult GetCommentIDs([FromQuery] CommentsIDsGet parameters)
        {
            var commentIDs = (from comments in _context.Comments
                              where comments.Postid == parameters.PostID
                              orderby comments.Id ascending
                              select comments.Id).ToList();

            return Ok(commentIDs);
        }

        [HttpGet]
        public IActionResult GetComment([FromQuery] CommentGet parameters)
        {
            var commentQuery = (from comments in _context.Comments
                                join users in _context.Users on comments.Userid equals users.Id
                                where comments.Id == parameters.CommentID
                                select new { comments, users }).Single();

            // 5 minute leeway before a comment is considered edited
            var isEdited = commentQuery.comments.Editdate.HasValue && commentQuery.comments.Editdate > commentQuery.comments.Createdon.AddMinutes(5);


            var comment = new
            {
                comment = commentQuery.comments.Comment,
                date = commentQuery.comments.Createdon.ToString("dddd, MMM dd, yyyy hh:mm tt"),
                isEdited,
                username = commentQuery.users.Username,
                userID = commentQuery.users.Id
            };

            return Ok(comment);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostComment(CommentPost parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);

            var comment = new Comments
            {
                Comment = parameters.Comment,
                Userid = userID,
                Postid = parameters.PostID,
                Createdon = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPatch]
        public IActionResult EditComment(CommentEditPatch parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);
            var role = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            var comment = (from comments in _context.Comments
                           where comments.Id == parameters.CommentID
                           select comments).Single();

            if (role != "Admin" && comment.Userid != userID)
                return BadRequest(new { error = "You do not have permission to edit this post" });

            comment.Editdate = DateTime.Now;
            comment.Comment = parameters.Comment;

            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteComment(CommentDelete parameters)
        {
            var userID = int.Parse(HttpContext.User.Claims.Single(c => c.Type == "UserID").Value);
            var role = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            var comment = (from comments in _context.Comments
                           where comments.Id == parameters.CommentID
                           select comments).Single();

            if (role != "Admin" && comment.Userid != userID)
                return BadRequest(new { error = "You do not have permission to delete this post" });

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Ok();
        }
    }
}