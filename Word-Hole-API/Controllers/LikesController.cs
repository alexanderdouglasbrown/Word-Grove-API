using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Word_Grove_API.Models.DB;
using Word_Grove_API.Models.Like;
using Word_Grove_API.Shared;

namespace Word_Grove_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly WordGroveDBContext _context;

        public LikesController(WordGroveDBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPut]
        public IActionResult AddLike(LikesPut parameters)
        {
            var userID = JWTUtility.GetUserID(HttpContext);

            var queryCheckAlreadyLiked = (from likes in _context.Likes
                                          where likes.Userid == userID
                                          && likes.Postid == parameters.PostID
                                          select likes).FirstOrDefault();

            if (queryCheckAlreadyLiked != null)
                return Ok(); // Ignore. No need to throw error.

            var newLike = new Likes()
            {
                Userid = userID,
                Postid = parameters.PostID
            };

            _context.Likes.Add(newLike);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult RemoveLike(LikesDelete parameters)
        {
            var userID = JWTUtility.GetUserID(HttpContext);

            var like = (from likes in _context.Likes
                             where likes.Userid == userID
                             && likes.Postid == parameters.PostID
                             select likes).FirstOrDefault();

            if (like == null)
                return Ok(); // Ignore. Probably a UI problem.

            _context.Likes.Remove(like);
            _context.SaveChanges();

            return Ok();
        }
    }
}