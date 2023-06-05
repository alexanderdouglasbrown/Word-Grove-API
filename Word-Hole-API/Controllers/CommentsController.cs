using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Word_Hole_API.Models.Comments;
using Word_Hole_API.Models.DB;
using Word_Hole_API.Shared;

namespace Word_Hole_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly WordGroveDBContext _context;

        public CommentsController(WordGroveDBContext context)
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
            var comment = (from comments in _context.Comments
                           where comments.Id == parameters.CommentID
                           select comments).Include(x => x.User).Single();

            // 5 minute leeway before a comment is considered edited
            var isEdited = comment.Editdate.HasValue && comment.Editdate > comment.Createdon.AddMinutes(5);

            var commentData = new
            {
                comment = comment.Comment,
                date = Common.GetPSTTimeString(comment.Createdon),
                isEdited,
                username = comment.User.Username,
                userID = comment.User.Id
            };

            return Ok(commentData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostComment(CommentPost parameters)
        {
            var userID = JWTUtility.GetUserID(HttpContext);

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
            var userID = JWTUtility.GetUserID(HttpContext);
            var role = JWTUtility.GetRole(HttpContext);

            var comment = (from comments in _context.Comments
                           where comments.Id == parameters.CommentID
                           select comments).Single();

            if (role != RoleType.Admin && comment.Userid != userID)
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
            var userID = JWTUtility.GetUserID(HttpContext);
            var role = JWTUtility.GetRole(HttpContext);

            var comment = (from comments in _context.Comments
                           where comments.Id == parameters.CommentID
                           select comments).Single();

            if (role != RoleType.Admin && comment.Userid != userID)
                return BadRequest(new { error = "You do not have permission to delete this post" });

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Ok();
        }
    }
}